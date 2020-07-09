namespace NewsFeedAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<NewsFeedAPI.Data.NewsFeedAPIContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NewsFeedAPI.Data.NewsFeedAPIContext context)
        {
            context.NewsInstances.AddOrUpdate(
                    n => n.Headline,
                    new NewsInstance
                    {
                        Headline = "Себастьян Феттель: Конечно, я огорчен",
                        Content = "В квалификации перед Гран При Австрии Себастьян Феттель " +
                                  "выбыл из борьбы уже во второй сессии, показав одиннадцатый результат.",
                        RateSum = 10,
                        RateCount = 2,
                        Category = "Автоспорт"
                    },

                    new NewsInstance
                    {
                        Headline = "Себастьян Феттель: «Мерседес» сейчас в своей собственной лиге",
                        Content = "Пилот «Феррари» Себастьян Феттель после пятничной серии свободных практик" +
                                  " Гран-при Австрии оценил готовность соперников к сезону-2020.",
                        RateSum = 5,
                        RateCount = 3,
                        Category = "Автоспорт"
                    },
                    new NewsInstance
                    {
                        Headline = "Официально: Фернандо Алонсо вернулся в «Рено»",
                        Content = "Французская команда «Формулы-1» «Рено» в среду официально объявила о возвращении испанского пилота Фернандо Алонсо. " +
                                  "38 - летний Алонсо будет пилотом команды в сезоне - 2021.",
                        RateSum = 5,
                        RateCount = 3,
                        Category = "Знаменитости"
                    }
                );
        }
    }
}
