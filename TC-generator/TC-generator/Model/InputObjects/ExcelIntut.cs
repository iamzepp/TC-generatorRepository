using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace TC_generator.Model.InputObjects
{
    public class ExcelIntut : IInput
    {
        //Книга Excel
        XSSFWorkbook xssfwb;

        public InputInfo GetInfo()
        {
            InputInfo inputInfo = new InputInfo();

            //Открываем файл
            using (FileStream file = new FileStream("csharp.XLSX", FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            
            inputInfo.ConnectArray = GetIntMassive(0, 3, 3);
            inputInfo.Q_RecuperatorArray = GetDoubleMassive(1, 3, 3);

            xssfwb.Close();

            return inputInfo;
        }

        public double[,] GetDoubleMassive(int index, int RowCount, int ColumnCount)
        {
            //Получаем первый лист книги
            ISheet sheet = xssfwb.GetSheetAt(index);

            double[,] Mas = new double[RowCount, ColumnCount];

            //запускаем цикл по строкам
            for (int row = 0; row <= RowCount; row++)
            {
                //получаем строку
                var currentRow = sheet.GetRow(row);
                if (currentRow != null) //null когда строка содержит только пустые ячейки
                {
                    //запускаем цикл по столбцам
                    for (int column = 0; column < ColumnCount; column++)
                    {
                        //получаем значение яейки
                        var stringCellValue = currentRow.GetCell(column).NumericCellValue;
                        Mas[row, column] = stringCellValue;
                    }
                }
            }

            return Mas;
        }

        public int[,] GetIntMassive(int index, int RowCount, int ColumnCount)
        {
            //Получаем первый лист книги
            ISheet sheet = xssfwb.GetSheetAt(index);

            int[,] Mas = new int[RowCount, ColumnCount];

            //запускаем цикл по строкам
            for (int row = 0; row <= RowCount; row++)
            {
                //получаем строку
                var currentRow = sheet.GetRow(row);
                if (currentRow != null) //null когда строка содержит только пустые ячейки
                {
                    //запускаем цикл по столбцам
                    for (int column = 0; column < ColumnCount; column++)
                    {
                        //получаем значение яейки
                        var stringCellValue = currentRow.GetCell(column).NumericCellValue;
                        Mas[row, column] = (int)stringCellValue;
                    }
                }
            }

            return Mas;
        }

    }
}
