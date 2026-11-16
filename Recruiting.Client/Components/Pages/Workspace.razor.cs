using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recruiting.Client.Components.Pages
{
    public partial class Workspace
    {
        [SupplyParameterFromQuery(Name = "vacId")]
        public string? VacancyId { get; set; }

        private string _search = "";
        private string _currentStatus = "all";
        private int _selectedCandId = 1;
        private string _selectedVacId = "vac-1";

        public record VacancyItem(string Id, string Title, int Count);
        private List<VacancyItem> _vacancies = new()
        {
            new("vac-1", "Senior Frontend-разработчик", 4),
            new("vac-2", "Менеджер по продажам B2B", 1),
            new("vac-3", "Продуктовый дизайнер", 1)
        };

        private void HandleVacancyChanged(ChangeEventArgs e)
        {
            _selectedVacId = e.Value?.ToString() ?? "vac-1";
        }

        private string GetResponsesWord(int count)
        {
            var mod10 = count % 10;
            var mod100 = count % 100;
            if (mod10 == 1 && mod100 != 11) return "отклик";
            if (mod10 >= 2 && mod10 <= 4 && (mod100 < 10 || mod100 >= 20)) return "отклика";
            return "откликов";
        }

        private readonly List<(string Key, string Title)> _statusTabs = new()
        {
            ("all", "Все"),
            ("new", "Новые"),
            ("work", "В работе"),
            ("reject", "Отказ")
        };

        public class CandModel
        {
            public int Id { get; set; }
            public string Position { get; set; } = "";
            public int Salary { get; set; }
            public string Name { get; set; } = "";
            public string City { get; set; } = "";
            public string Age { get; set; } = "";
            public string Status { get; set; } = "new";
            public string LastJob { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Telegram { get; set; } = "";
            public string Experience { get; set; } = "";
            public string[] Skills { get; set; } = Array.Empty<string>();

            public CandModel(int id, string position, int salary, string name, string city, string age, string status, string lastJob, string phone, string telegram, string experience, string[] skills)
            {
                Id = id;
                Position = position;
                Salary = salary;
                Name = name;
                City = city;
                Age = age;
                Status = status;
                LastJob = lastJob;
                Phone = phone;
                Telegram = telegram;
                Experience = experience;
                Skills = skills;
            }
        }

        private List<CandModel> _list = new()
        {
            new(1, "Senior Frontend-разработчик", 260000, "Смирнов Алексей Викторович", "Москва", "29 лет", "new", "АО «Финтех Решения»", "+7 (916) 548-21-90", "@alex_smirnov", "5 лет и 4 месяца", new[] { "React", "TypeScript", "Next.js", "Tailwind CSS", "Git" }),
            new(2, "Frontend-разработчик / Teamlead", 280000, "Морозов Денис Андреевич", "Москва", "32 года", "work", "CloudRetail", "+7 (926) 804-11-37", "@dmorozov", "7 лет и 2 месяца", new[] { "TypeScript", "React 18", "GraphQL", "Vite" }),
            new(3, "Frontend-разработчик (React)", 230000, "Кузнецова Анна Сергеевна", "Санкт-Петербург", "27 лет", "new", "МедТех Сервис", "+7 (921) 409-63-12", "@anna_kuz", "4 года и 1 месяц", new[] { "React", "JavaScript", "Figma" }),
            new(4, "Junior+ / Middle Frontend", 190000, "Волков Максим Олегович", "Москва", "25 лет", "reject", "ВебСтудия Старт", "+7 (905) 773-19-40", "@max_volkov", "1 год и 9 месяцев", new[] { "JavaScript", "React", "SCSS" })
        };

        private IEnumerable<CandModel> FilteredList =>
            _list.Where(c =>
                (_currentStatus == "all" || c.Status == _currentStatus) &&
                (string.IsNullOrWhiteSpace(_search) ||
                 c.Name.Contains(_search, StringComparison.OrdinalIgnoreCase) ||
                 c.Position.Contains(_search, StringComparison.OrdinalIgnoreCase)));

        private CandModel? CurrentCand => _list.FirstOrDefault(c => c.Id == _selectedCandId);

        private string GetStatusName(string st) => st switch
        {
            "new" => "Новый",
            "work" => "В работе",
            "reject" => "Отказ",
            _ => st
        };
    }
}
