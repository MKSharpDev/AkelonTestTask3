using AkelonTestTask3.ExcelContext;
using AkelonTestTask3.Services;


bool fileExist = false;
string filePath = "";
while (!fileExist)
{
    Console.WriteLine("Введите путь файла Exel");
    filePath = Console.ReadLine();

    if (File.Exists(filePath))
    {
        fileExist = true;
    }
    else
    {
        Console.WriteLine("Файл отсутствует");
    }
}
var context = new ExelRepository(filePath);

AppService appService = new AppService(context);
appService.Run();

