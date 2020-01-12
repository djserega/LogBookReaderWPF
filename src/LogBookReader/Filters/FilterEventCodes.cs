namespace LogBookReader.Filters
{
    public class FilterEventCodes : IModels.IEventCodes, IFilters.IFilterBase
    {
        public FilterEventCodes(Models.EventCodes eventCodes)
        {
            Fill(eventCodes);
        }

        public bool IsChecked { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public void Fill(Models.EventCodes eventCodes, bool translateName = true)
        {
            Code = eventCodes.Code;
            Name = eventCodes.Name.Replace("\"", "");
            if (translateName)
                Name = EventNameRu(Name);
        }

        public static string EventNameRu(string nameEn)
        {
            string result;

            switch (nameEn)
            {
                case "_$Access$_.Access": result = "Доступ. Доступ"; break;
                case "_$Access$_.AccessDenied": result = "Доступ. Отказ в доступе"; break;

                case "_$Data$_.Delete": result = "Данные.Удаление"; break;
                case "_$Data$_.New": result = "Данные.Добавление"; break;
                case "_$Data$_.Post": result = "Данные.Проведение"; break;
                case "_$Data$_.TotalsMaxPeriodUpdate": result = "Данные.Изменение максимального периода рассчитанных итогов"; break;
                case "_$Data$_.TotalsMinPeriodUpdate": result = "Данные.Изменение минимального периода рассчитанных итогов"; break;
                case "_$Data$_.Unpost": result = "Данные.Отмена проведения"; break;
                case "_$Data$_.Update": result = "Данные.Изменение"; break;
                case "_$Data$_.PredefinedDataInitialization": result = "Данные.Предопределенные данные созданы"; break;
                case "_$Data$_.SetStandardODataInterfaceContent": result = "Данные. Изменение состава стандартного интерфейса OData"; break;

                case "_$InfoBase$_.ConfigUpdate": result = "Информационная база.Изменение конфигурации"; break;
                case "_$InfoBase$_.DBConfigUpdate": result = "Информационная база.Изменение конфигурации базы данных"; break;
                case "_$InfoBase$_.DBConfigBackgroundUpdateStart": result = "Информационная база.Запуск фонового обновления"; break;
                case "_$InfoBase$_.DBConfigBackgroundUpdateFinish": result = "Информационная база.Завершение фонового обновления"; break;
                case "_$InfoBase$_.DBConfigBackgroundUpdateCancel": result = "Информационная база.Отмена фонового обновления"; break;
                case "_$InfoBase$_.DBConfigBackgroundUpdateSuspend": result = "Информационная база.Приостановка(пауза) процесса фонового обновления"; break;
                case "_$InfoBase$_.DBConfigBackgroundUpdateResume": result = "Информационная база.Продолжение(после приостановки) процесса фонового обновления"; break;
                case "_$InfoBase$_.EventLogSettingsUpdate": result = "Информационная база.Изменение параметров журнала регистрации"; break;
                case "_$InfoBase$_.InfoBaseAdmParamsUpdate": result = "Информационная база.Изменение параметров информационной базы"; break;
                case "_$InfoBase$_.MasterNodeUpdate": result = "Информационная база.Изменение главного узла"; break;
                case "_$InfoBase$_.PredefinedDataUpdate": result = "Информационная база.Выполняется обновление предопределенных данных"; break;
                case "_$InfoBase$_.RegionalSettingsUpdate": result = "Информационная база.Изменение региональных установок"; break;
                case "_$InfoBase$_.EraseData": result = "Информационная база.Удаление данных информационной баз"; break;
                case "_$InfoBase$_.TARImportant": result = "Тестирование и исправление. Ошибка"; break;
                case "_$InfoBase$_.TARInfo": result = "Тестирование и исправление. Сообщение"; break;
                case "_$InfoBase$_.TARMess": result = "Тестирование и исправление. Предупреждение"; break;

                case "_$Job$_.Cancel": result = "Фоновое задание.Отмена"; break;
                case "_$Job$_.Fail": result = "Фоновое задание.Ошибка выполнения"; break;
                case "_$Job$_.Start": result = "Фоновое задание.Запуск"; break;
                case "_$Job$_.Succeed": result = "Фоновое задание.Успешное завершение"; break;

                case "_$PerformError$_": result = "Ошибка выполнения"; break;

                case "_$Session$_.Authentication": result = "Сеанс.Аутентификация"; break;
                case "_$Session$_.AuthenticationError": result = "Сеанс.Ошибка аутентификации"; break;
                case "_$Session$_.Finish": result = "Сеанс.Завершение"; break;
                case "_$Session$_.Start": result = "Сеанс.Начало"; break;

                case "_$Transaction$_.Begin": result = "Транзакция.Начало"; break;
                case "_$Transaction$_.Commit": result = "Транзакция.Фиксация"; break;
                case "_$Transaction$_.Rollback": result = "Транзакция.Отмена"; break;

                case "_$User$_.Delete": result = "Пользователи.Удаление"; break;
                case "_$User$_.New": result = "Пользователи.Добавление"; break;
                case "_$User$_.Update": result = "Пользователи. Изменение"; break;

                case "_$OpenIDProvider$_.PositiveAssertion": result = "Провайдер OpenID. Подтверждено"; break;
                case "_$OpenIDProvider$_.NegativeAssertion": result = "Провайдер OpenID. Отклонено"; break;

                default: result = nameEn; break;
            }

            return result;
        }

    }
}
