using HRManagementSystem.BL.DTOs.ChatDTO;
using HRManagementSystem.BL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IPayRollService _payrollService;

        private readonly string _huggingFaceToken;

        public ChatbotService(IAttendanceService attendanceService, IEmployeeService employeeService, IDepartmentService departmentService, IPayRollService payRollService, IConfiguration configration)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
            _departmentService = departmentService;
            _payrollService = payRollService;

            _huggingFaceToken = configration["HuggingFace:Token"] ?? throw new ArgumentNullException(nameof(configration), "HuggingFace:Token configuration is missing.");
        }

        public async Task<string> GetChatReplyAsync(chatDto chat)
        {
            var message = chat.Message.ToLower();
            if (string.IsNullOrEmpty(message)) return "Please provide a valid message.";

            var employees = await _employeeService.GetAllEmployeesAsync();
            var knownNames = employees.Select(e => e.Name?.ToLower())
                              .Where(n => !string.IsNullOrWhiteSpace(n))
                              .Distinct()
                              .ToList();

            var departments = await _departmentService.GetAllDepartmentsAsync();
            var knownDepartments = departments.Select(d => d.Name?.ToLower())
                                      .Where(d => !string.IsNullOrWhiteSpace(d))
                                      .Distinct()
                                      .ToList();

            string? foundName = knownNames.FirstOrDefault(n =>
                message.Contains(n!) || n!.Split(' ').Any(part => message.Contains(part)));

            var foundDept = knownDepartments.FirstOrDefault(d => message.Contains(d!));

            // Attendance
            if (message.Contains("attendance"))
            {
                if (!string.IsNullOrEmpty(foundName))
                {
                    var records = await _attendanceService.GetAllFilteredAsync(foundName, null, null);
                    return records.Any()
                        ? $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(foundName)} has {records.Count()} attendance days"
                        : $"No attendance found for {foundName}.";
                }
                if (message.Contains("all") || message.Contains("total"))
                {
                    var all = await _attendanceService.GetAllAttendancesAsync();
                    return $"There are {all.Count()} total attendance records.";
                }
                if (string.IsNullOrEmpty(foundName))
                {
                    return "Sorry, I couldn't find any employee with that name in the system.";
                }
            }

            // Employees by department
            if (message.Contains("employees") &&
               (message.Contains("how many") || message.Contains("list") || message.Contains("show") || message.Contains("who")))
            {
                if (!string.IsNullOrEmpty(foundDept))
                {
                    var deptEmployees = await _employeeService.GetEmployeesByDepartmentNameAsync(foundDept);
                    if (!deptEmployees.Any()) return $"No employees found in the {foundDept} department.";

                    return message.Contains("how many")
                        ? $"There are {deptEmployees.Count()} employees in the {foundDept} department."
                        : $"Employees in the {foundDept} department: {string.Join(", ", deptEmployees.Select(e => e.Name))}.";
                }

                var all = await _employeeService.GetAllEmployeesAsync();
                return message.Contains("how many")
                    ? $"There are {all.Count()} employees in the system."
                    : $"All employees: {string.Join(", ", all.Select(e => e.Name))}.";
            }

            // Salary check
            if (message.Contains("salary") && !string.IsNullOrEmpty(foundName) && message.Contains("month"))
            {
                if (string.IsNullOrEmpty(foundName))
                {
                    return "Sorry, I couldn't find any employee with that name in the system.";
                }
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;

                var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for (int i = 0; i < months.Length; i++)
                {
                    if (message.Contains(months[i].ToLower()))
                    {
                        month = i + 1;
                        break;
                    }
                }

                if (message.Contains("last month")) month = DateTime.Now.AddMonths(-1).Month;
                if (message.Contains("this month")) month = DateTime.Now.Month;

                if (message.Contains("this year")) year = DateTime.Now.Year;
                if (message.Contains("last year")) year = DateTime.Now.AddYears(-1).Year;

                var payrolls = await _payrollService.GetPaginatedAttendancesAsync(1, 10, foundName, month, year);
                var salaryRecord = payrolls.Items.FirstOrDefault();
                return salaryRecord == null
                    ? $"No salary record found for {foundName} for {month}/{year}."
                    : $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(foundName)} has a salary of {salaryRecord.NetSalary} EGP for {month}/{year}.";
            }
            return await GetAIResponse(message);
        }

        private async Task<string> GetAIResponse(string message)
        {
            using var httpClient = new HttpClient();
            var apiUrl = "https://router.huggingface.co/together/v1/chat/completions";
            var payload = new
            {
                model = "mistralai/Mixtral-8x7B-Instruct-v0.1",
                messages = new[]
                {
                        new { role = "system", content = "You are a helpful assistant answering HR-related questions." },
                        new { role = "user", content = message }
                    },
                temperature = 0.7,
                max_tokens = 512
            };
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _huggingFaceToken);
            request.Content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return "Sorry, I couldn't understand your request.";

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "No response.";
        }
    
    }
}
