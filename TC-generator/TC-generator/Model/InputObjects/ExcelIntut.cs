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

        public InputInfo GetInfo(string name)
        {
            //Открываем файл
            using (FileStream file = new FileStream(name, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }

            Random random = new Random();

            InputInfo inputInfo = new InputInfo();

            var InitialData = GetInitialData(0);
            
            inputInfo.StudyCount = InitialData.Study;
            inputInfo.ColdFlowCount = InitialData.ColdCount;
            inputInfo.HotFlowCount = InitialData.HotCount;

            var DataTH = GetTemperaturesData(1, inputInfo.HotFlowCount);
            var DataTC = GetTemperaturesData(2, inputInfo.HotFlowCount);

            for (int i = 0; i < inputInfo.ColdFlowCount; i++)
            {
                inputInfo.CF_Tk.Add(DataTC.TCK[i]);
                inputInfo.CF_Tn.Add(DataTC.TCN[i]);
            }

            for (int i = 0; i < inputInfo.HotFlowCount; i++)
            {
                inputInfo.HF_Tk.Add(DataTH.TCK[i]);
                inputInfo.HF_Tn.Add(DataTH.TCN[i]);
            }

            inputInfo.StartInitial();

            
            inputInfo.ConnectArray = GetIntMassive(3, inputInfo.x, inputInfo.y);
            inputInfo.IntTypeConnect = GetIntMassive(4, inputInfo.x, inputInfo.y);

            inputInfo.Q_RecuperatorArray = GetDoubleMassive(5, inputInfo.x, inputInfo.y);
            inputInfo.Q_CoolerArray = GetDoubleMassive(6, inputInfo.x, inputInfo.y);
            inputInfo.Q_HeaterArray = GetDoubleMassive(7, inputInfo.x, inputInfo.y);

            inputInfo.F_RecuperatorArray = GetDoubleMassive(8, inputInfo.x, inputInfo.y);
            inputInfo.F_CoolerArray = GetDoubleMassive(9, inputInfo.x, inputInfo.y);
            inputInfo.F_HeaterArray = GetDoubleMassive(10, inputInfo.x, inputInfo.y);

            for (int i = 0; i < (inputInfo.StudyCount * inputInfo.HotFlowCount); i++)
            {
                for (int j = 0; j < (inputInfo.StudyCount * inputInfo.ColdFlowCount); j++)
                {
                    switch (inputInfo.IntTypeConnect[i, j])
                    {
                        case 1:
                            inputInfo.Type[i, j] = ConnectType.V1;
                            break;
                        case 2:
                            inputInfo.Type[i, j] = ConnectType.V2;
                            break;
                        case 3:
                            inputInfo.Type[i, j] = ConnectType.V3;
                            break;
                        case 4:
                            inputInfo.Type[i, j] = ConnectType.V4;
                            break;
                    }
                }
            }

            xssfwb.Close();

            return inputInfo;
        }

        public double[,] GetDoubleMassive(int index, int RowCount, int ColumnCount)
        {
            //Получаем первый лист книги
            ISheet sheet = xssfwb.GetSheetAt(index);

            double[,] Mas = new double[RowCount, ColumnCount];

            //запускаем цикл по строкам
            for (int row = 0; row < RowCount; row++)
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
            for (int row = 0; row < RowCount; row++)
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

        public (int Study, int HotCount, int ColdCount) GetInitialData(int index)
        {
            ISheet sheet = xssfwb.GetSheetAt(index);
            var currentRow = sheet.GetRow(0);

            int a = (int) currentRow.GetCell(0).NumericCellValue;
            int b = (int)currentRow.GetCell(1).NumericCellValue;
            int c = (int)currentRow.GetCell(2).NumericCellValue;

            return (a, b, c);
        }

        public (int[] TCN, int[] TCK) GetTemperaturesData(int index, int n)
        {
            //Получаем первый лист книги
            ISheet sheet = xssfwb.GetSheetAt(index);

            int[] a = new int[n];
            int[] b = new int[n];

            var currentRow = sheet.GetRow(0);

            for (int i = 0; i< n; i++)
            {
               a[i]  = (int)currentRow.GetCell(i).NumericCellValue;
            }

            currentRow = sheet.GetRow(1);

            for (int i = 0; i < n; i++)
            {
                b[i] = (int)currentRow.GetCell(i).NumericCellValue;
            }

            return (a,b);
        }

    }
}
