using System.Collections.Generic;
using FocusAccess;
using FocusScoring;

namespace FocusMarkers
{
    public class CodeMarkerProvider : IMarkersProvider<INN>
    {
        public Marker<INN>[] Markers => new[]
        {
            new Marker<INN>
            {
                Name = "Тест",
                Description = "",
                Colour = MarkerColour.Red,
                Score = 5,
                Methods = new[]{ ApiMethodEnum.req, ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker0"}}
            },
            new Marker<INN>
            {
                Name = "Деятельность предприятия убыточна",
                Description = "Отрицательная чистая прибыль на конец отчетного периода (за последний отчетный год, оценка в рублях).",
                Colour = MarkerColour.Red,
                Score = 5,
                Methods = new[]{ApiMethodEnum.req,ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker1"}}
            },
            new Marker<INN>
            {
                Name = "Есть записи о банкротстве физ. лица",
                Description = "",
                Colour = MarkerColour.Red,
                Score = 5,
                Methods = new[]{ ApiMethodEnum.req},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker2"}}
            }, 
            new Marker<INN>
            {
                Name = "Обнаружены сообщения о текущей процедуре банкротства (стадия)",
                Description = "Обнаружены сообщения о текущей процедуре банкротства (стадия) за последние 12 месяцев.",
                Colour = MarkerColour.Red,
                Score = 5,
                Methods = new[]{ ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker3"}}
            },
            new Marker<INN>
            {
                Name = "Намерение подать иск о банкротстве",
                Description = "Обнаружены сообщения о намерении обратиться в суд с заявлением о банкротстве за последние 3 месяца.",
                Colour = MarkerColour.Red,
                Score = 3,
                Methods = new[]{ ApiMethodEnum.req},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker4"}}
            }, 
            new Marker<INN>
            {
                Name = "Намерение подать иск о банкротстве",
                Description = "Обнаружены сообщения о намерении обратиться в суд с заявлением о банкротстве за последние 3 месяца.",
                Colour = MarkerColour.Red,
                Score = 3,
                Methods = new[]{ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker5"}}
            }, 
            new Marker<INN>
            {
                Name = "Обнаружены арбитражные дела о банкротстве в качестве ответчика",
                Description = "Обнаружены арбитражные дела о банкротстве в качестве ответчика (наличие дела о банкротстве не обязательно свидетельствует о начале процедуры банкротства).",
                Colour = MarkerColour.Red,
                Score = 4,
                Methods = new[]{ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker6"}}
            }, 
            new Marker<INN>
            {
                Name = "Исполнительные производства по заработной плате",
                Description = "У организации были найдены исполнительные производства, предметом которых является заработная плата.",
                Colour = MarkerColour.Red,
                Score = 3,
                Methods = new[]{ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker7"}}
            }, 
            new Marker<INN>
            {
                Name = "Организация в реестре недобросовестных поставщиков",
                Description = "Организация была найдена в реестре недобросовестных поставщиков (ФАС, Федеральное Казначейство).",
                Colour = MarkerColour.Red,
                Score = 3,
                Methods = new[]{ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker8"}}
            }, 
            new Marker<INN>
            {
                Name = "Организация в санкционных списках",
                Description = "Организация найдена в одном или нескольких санкционных списках: США, секторальном списке США, Евросоюза, Великобритании, Украины, Швейцарии.",
                Colour = MarkerColour.Red,
                Score = 3,
                Methods = new[]{ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker9"}}
            }, 
            new Marker<INN>
            {
                Name = "Руководство в реестре дисквалифицированных лиц",
                Description = "ФИО руководителей были найдены в реестре дисквалифицированных лиц (ФНС) или в выписке ЕГРЮЛ.",
                Colour = MarkerColour.Red,
                Score = 3,
                Methods = new[]{ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker11"}}
            }, 
            new Marker<INN>
            {
                Name = "Блокировка банковского счета организации",
                Description = "По состоянию на указанную дату действовало ограничение на операции по банковским счетам организации, установленное ИФНС. Это означает, что в отношении данной организации ИФНС отправляла в банки своё решение о частичном или полном прекращении расходных операций по счетам (ст. 76 НК РФ). В настоящий момент это решение может быть уже отменено, а расходные операции разрешены. Рекомендуем проверить текущее состояние банковских счетов. Обращаем внимание: маркер может сработать, только если блокировка счета организации ранее была проверена на сайте focus.kontur.ru. Если у организации никто никогда не проверял блокировку счета на сайте focus.kontur.ru - это равнозначно отсутствию информации, но не гарантирует, что заблокированных счетов нет.",
                Colour = MarkerColour.Red,
                Score = 2,
                Methods = new[]{ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker12"}}
            }, 
            new Marker<INN>
            {
                Name = "Обнаружены признаки завершенной процедуры банкротства",
                Description = "Обнаружены признаки завершенной процедуры банкротства.",
                Colour = MarkerColour.Red,
                Score = 2,
                Methods = new[]{ ApiMethodEnum.req, ApiMethodEnum.analytics},
                CheckArguments = new Dictionary<string, string>{{"LibraryCheckMethodName","Marker13"}}
            },
        };~
    }
}