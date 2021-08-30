using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using System.Net;
using System.IO;

namespace StoneX
{
    public class Program
    {
        [Serializable, XmlRoot("Valuta")]
        public class Valuta
        {
            [XmlElement("Item")]
            public Item[] Item { get; set; }           
        }

        static void Main(string[] args)
        {
            // заполнение справочника
            LoadDir();
            
            // получение курса на сегодня
            string URLString = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + DateTime.Now.ToShortDateString();
            XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
            WebClient client = new WebClient();
            string data = Encoding.Default.GetString(client.DownloadData(URLString));
            Stream stream = new MemoryStream(Encoding.Default.GetBytes(data));
            
            ValCurs cur = (ValCurs)serializer.Deserialize(stream);
            using (DBModel db = new DBModel())
            {
                string form = DateTime.Parse(cur.Date).ToString("yyyy-MM-dd");
                db.Database.ExecuteSqlCommand("DELETE FROM StoneX.dbo.CurrencyDaily WHERE date = '" + form + "'");
                foreach (Valute u in cur.Valute)
                {
                    db.CurrencyDaily.Add(new CurrencyDaily { currency_id = u.ID, date = DateTime.Parse(cur.Date), value = decimal.Parse(u.Value) });
                }
                db.SaveChanges();
                Console.WriteLine("Курс на " + cur.Date + " получен.");
            }

            // получение курса на дату
            Console.WriteLine("Введите желаемую валюту:");
            string val = Console.ReadLine();
            Console.WriteLine("Введите желаемую дату:");
            string date = Console.ReadLine();
            decimal currency = LoadCurrencyDaily(val, date);
            Console.WriteLine("Курс " + val + " = " + currency);
        }

        // функция для получения курса на дату
        static decimal LoadCurrencyDaily(string val, string date)
        {
            try
            {
                string dateAct = DateTime.Parse(date).ToShortDateString();
                string URLString = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + dateAct;

                XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
                WebClient client = new WebClient();

                string data = Encoding.Default.GetString(client.DownloadData(URLString));
                Stream stream = new MemoryStream(Encoding.Default.GetBytes(data));

                ValCurs cur = (ValCurs)serializer.Deserialize(stream);
                // запись в таблицу
                using (DBModel db = new DBModel())
                {
                    string form = DateTime.Parse(cur.Date).ToString("yyyy-MM-dd");
                    db.Database.ExecuteSqlCommand("DELETE FROM StoneX.dbo.CurrencyDaily WHERE date = '" + form + "'");
                    foreach (Valute u in cur.Valute)
                    {
                        db.CurrencyDaily.Add(new CurrencyDaily { currency_id = u.ID, date = DateTime.Parse(cur.Date), value = decimal.Parse(u.Value) });
                    }
                    db.SaveChanges();
                }
                
                Valute result = cur.Valute.FirstOrDefault(x => x.Name == val || x.ID == val);
                if (result == null)
                {
                    Console.WriteLine("Данная валюта не найдена!");
                    return 0;
                }
                else return decimal.Parse(result.Value);
            }
            catch(FormatException)
            {
                Console.WriteLine("Ошибка при вводе даты!");
                return 0;
            }
            catch(System.Data.Entity.Infrastructure.DbUpdateException)
            {
                Console.WriteLine("Слишком старая дата, ID этой валюты отсутствует в словаре.");
                return 0;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Дата указана в будущем!");
                return 0;
            }
        }

        // загрузка справочника валют
        static void LoadDir()
        {
            using (DBModel db = new DBModel())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM StoneX.dbo.Currency");
                
                string URLString = "http://www.cbr.ru/scripts/XML_valFull.asp";

                XmlSerializer serializer = new XmlSerializer(typeof(Valuta));
                WebClient client = new WebClient();

                string data = Encoding.Unicode.GetString(client.DownloadData(URLString));
                Stream stream = new MemoryStream(Encoding.Unicode.GetBytes(data));

                Valuta cur = (Valuta)serializer.Deserialize(stream);

                db.Currency.AddRange(cur.Item);
                db.SaveChanges();
                Console.WriteLine("Справочник заполнен");
            }
        }

        [Serializable, XmlRoot("ValCurs")]
        public class ValCurs
        {
            [XmlAttribute("Date")]
            public string Date { get; set; }

            [XmlElement("Valute")]
            public Valute[] Valute { get; set; }
        }

        [Serializable, XmlType("Valute")]
        public partial class Valute
        {
            public Valute()
            {   }

            [XmlAttribute("ID")]
            public string ID { get; set; }

            [XmlElement("Name")]
            public string Name { get; set; }

            [XmlElement("NumCode")]
            public string NumCode { get; set; }

            [XmlElement("CharCode")]
            public string CharCode { get; set; }

            [XmlElement("Nominal")]
            public int? Nominal { get; set; }

            [XmlElement("Value")]
            public string Value { get; set; }

            public Valute(string _id, string _name, string _num_code, string _char_code, int? _nominal, string _value)
            {
                ID = _id;
                Name = _name;
                NumCode = _num_code;
                CharCode = _char_code;
                Nominal = _nominal;
                Value = _value;
            }
        }
    }
}
