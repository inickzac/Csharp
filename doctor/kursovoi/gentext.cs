using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace terapevt
{
    class gentext//класс генерирует html страницу для контрола webbrouser
    {
      public  int textSize = 10;           //поля инициализации документа
        string FIO = "ФИО: ";
        public string FioField = "";
        string sex = "<br>Пол: ";
        public string sexField= "";
        string AgeDate = " Дата рождения: " ;
        public string ageDateField = "";
        string adress = " Адрес: ";
        public string AdressField = "";
        string footer = "</body> </html>";
        string inspectionDoctor = "<br> Осмотр терапевта<br>";
        public string dateInspection = "";
        string inspectionString = "<br> Осмотр на приеме Цель обращения по заболеванию Форма " +
            "обслуживания бесплатная услуга";
        string complains = "<br>Жалобы: ";
        public string complainsField = "";
        string incpectionField = "<br>Осмотрен(а) на чесотку, на педикулез. Опрошен(а) на тениоз.";
        string anamnez = "<br>Анамнез: ";
        public string anamnezField = "";
        string dataObjectiveInspection = "<br><b>Данные объективного обследования</b>";
        string generalState = "<br>Общее состояние: ";
        public string generalStateField = "";
        string temperature = " Температура: ";
        public string temperatureField = "";
        string weight = "Вес: ";
        public string weightField = "";
        string growth = " Рост: ";
        public string growthField = "";
        string IMT = " ";
        public string IMTField = "";
        public string IMTFieldString = "";
        string feeling = "Сознание: ";
        public string feelingField = "";
        string Onko = "<br><b>Профилактический онкоосмотр:</b>";
        public string cell1 = ""; 
        public string cell2 = "";
        public string cell3 = "";
        public string cell4 = "";
        public string cell5 = "";
        public string cell6 = "";
        public string cell7 = "";
        public string cell8 = "";
        public string cell9 = "";
        public string kozaOsmotr = "";
        public string vidimSlizist = "";
        public string mindalini = "";
        public string limfoUzli = "";
        public string hitovidZeleza = "";
        public string kosti = "";
        public string misci = "";
        public string legkie = "";
        public string dihShum = "";
        public string krovSistem = "";
        public string serdce = "";
        public string iazik = "";
        public string zivod = "";
        public string pechen = "";
        public string selizenka = "";
        public string pochki = "";
        public string mocheIspuskanie = "";
        public string styl = "";
        public string oteki = "";
        public string diagnoz = "";
        public string naznachenia = "";
        public string recepti = "";
        public string lgotnieRecepti = "";
        public string recomendacii = "";
        public string gruppaNabludenia = "";
        public string listNetrudosposobnosti = "";
        public string skfValue="";

        public string compleateText()    //генерируем полную html строку      
        {          
            string textHtml=headHtml() +FIO+ FioField +  sex + sexField+ AgeDate +ageDateField+
                adress+AdressField+ inspectionDoctor+ dateInspection+ inspectionString+ complains+ 
               tToBr (complainsField)+ incpectionField + anamnez+ tToBr(anamnezField) + dataObjectiveInspection+ 
               generalState+ generalStateField+ temperature+ temperatureField+ weight+ weightField+ growth
               + growthField+ IMT+IMTField+ IMTFieldString+ feeling+ feelingField+ Onko+ genOncoTable()+
               "Кожные покровы: "+ kozaOsmotr+ "<br/> Видимые слизистые: "+ vidimSlizist+ "<br/> Небные миндалины: "
              + mindalini+ "<br/> Лимфатические узлы: "+limfoUzli + "<br/> Щитовидная железа: "+ hitovidZeleza+
             "<br/>Костно - суставная система: " + kosti + "<br/>Мышечная система: "+ misci + "<br/>Легкие: "+
            legkie + "<br/> Дыхательные шумы: " + dihShum + "<br/> Пульс: " + krovSistem+ "<br/> Сердце: " +
            serdce+ "<br/> Язык: " +iazik + "<br/> Живот: "+ zivod + "<br/> Печень: " + pechen+ 
           "<br/> Селезенка: "+selizenka + "<br/> Почки: " +pochki+ skf(skfValue)+ "<br/> Мочеиспускание: " +mocheIspuskanie+
          "<br/>Стул: " +styl+ "<br/>Отеки: " + oteki + "<br/><b>Диагноз: </b>" + tToBr(diagnoz)+ "<br/> Направления: "+
         tToBr(naznachenia) + "<br/> Рецепты: "+ tToBr(recepti) + "<br/> Льготные рецепты: " + tToBr(lgotnieRecepti) + "<br/> Рекомендации: "
         + tToBr(recomendacii) + "<br/> Группа «Д» наблюдения: "+gruppaNabludenia + "<br/>" 
         + listNetrudosposobnosti+ " <br/>Врач общей практики "+ probel(60) + "Якубец И.В" +footer;
            return textHtml;

        }

        public string genOncoTable()//генерация онко таблицы
        {
           
            string compleat = "<table border=\"1\" bordercolor=\"black\"> <tr> <td>"+cell1+"</td> <td>"+cell2+ "</td></tr><tr> <td>" + cell3 + "</td>" +
                " <td>" + cell4 + "</td></tr><tr> <td>" + cell5 + "</td> <td>" + cell6 + "</td></tr> " +
                "<tr> <td>" + cell7 + "</td> <td></td></tr><tr> <td>" + cell8 + "</td> <td></td></tr>" +
                "<tr> <td>" + cell9 + "</td> <td></td></tr></table>";
            return compleat;
        }

        string tToBr(string t)//делаем новую строку html
        {
            t = t.Replace("\n", "<br/>");
            return t;
        }

        string probel( int count)//генерируем дополнительные пробелы
        {
            string str="";
            for (int i=0;i<=count;i++)
            {
                str += "&nbsp ";
            }

            return str; }

        public void OpenToWord()//открываем файл word
        {
            System.IO.DirectoryInfo dirinfo = new System.IO.DirectoryInfo(Application.StartupPath.ToString()+"\\temp");
            if(!dirinfo.Exists)
            { dirinfo.Create(); }
            string tempfile ="temp\\"+ Guid.NewGuid().ToString() + ".html";
            System.IO.File.WriteAllText(tempfile, compleateText());
            var word = new Microsoft.Office.Interop.Word.Application();
            word.Visible = true;
            var filepath = Application.StartupPath.ToString()+"\\"+tempfile;
            var worddoc = word.Documents.Open(FileName: filepath, ReadOnly: false);
        }

        string genCss()//генерируем css стиль
        { string css = "";

            string tableBorder = " table {border-collapse: collapse;} ";
            string textSizeCss = "body {font-size: "+textSize+"pt}";
            css ="<style type=\"text/css\">"+textSizeCss+tableBorder+"</style>";
            return css;
        }

        string headHtml()//генерируем шапку html
        {
            string headHtml = "<!DOCTYPE HTML>  <html> <head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /> " +
             "<title>Ваш сайт</title>" +genCss()+"</head> <body>";
            return headHtml;
        }

        string skf(string skf)//если не высчитано СКФ, то не выводим
        {
            string skfHtml="";
            if (skf!="" && skf!="0")          
            skfHtml = "<br/> СКФ: " + skf + " ";
            return skfHtml;   
        }

    }

    
   


}


