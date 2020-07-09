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
                        Headline = "��������� �������: �������, � �������",
                        Content = "� ������������ ����� ���� ��� ������� ��������� ������� " +
                                  "����� �� ������ ��� �� ������ ������, ������� ������������ ���������.",
                        RateSum = 10,
                        RateCount = 2,
                        Category = "���������"
                    },

                    new NewsInstance
                    {
                        Headline = "��������� �������: ��������� ������ � ����� ����������� ����",
                        Content = "����� �������� ��������� ������� ����� ��������� ����� ��������� �������" +
                                  " ����-��� ������� ������ ���������� ���������� � ������-2020.",
                        RateSum = 5,
                        RateCount = 3,
                        Category = "���������"
                    },
                    new NewsInstance
                    {
                        Headline = "����������: �������� ������ �������� � �����",
                        Content = "����������� ������� ��������-1� ����� � ����� ���������� �������� � ����������� ���������� ������ �������� ������. " +
                                  "38 - ������ ������ ����� ������� ������� � ������ - 2021.",
                        RateSum = 5,
                        RateCount = 3,
                        Category = "������������"
                    }
                );
        }
    }
}
