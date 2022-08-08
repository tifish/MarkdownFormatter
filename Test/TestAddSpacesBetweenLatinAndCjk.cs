using MarkdownFormatter.Formatters;

namespace Test;

[TestClass]
public class TestAddSpacesBetweenLatinAndCjk
{
    private void Test(string input, string output)
    {
        var formatter = new AddSpacesBetweenLatinAndCjk();
        var lines = new List<string> { input };
        formatter.Format(lines, "");
        Assert.AreEqual(output, lines[0]);
    }

    [TestMethod]
    public void TestCharactersAndPunctuations()
    {
        Test("WASAPI独占模式", @"WASAPI 独占模式");
        Test("独占模式WASAPI独占模式", @"独占模式 WASAPI 独占模式");
        Test("WASAPI独占模式WASAPI", @"WASAPI 独占模式 WASAPI");

        Test("(WASAPI)独占模式", "(WASAPI) 独占模式");
        Test("WASAPI(独占模式)", "WASAPI(独占模式)");
        Test("WASAPI（独占模式）", "WASAPI（独占模式）");
        Test("（WASAPI）独占模式", "（WASAPI）独占模式");

        Test("WASAPI，独占模式", "WASAPI，独占模式");
        Test("WASAPI,独占模式", "WASAPI,独占模式");

        Test("<http://WASAPI独占模式.com>", "<http://WASAPI独占模式.com>");
        Test("独占模式<http://WASAPI独占模式.com>独占模式", "独占模式 <http://WASAPI独占模式.com> 独占模式");
        Test("WASAPI<http://WASAPI独占模式.com>WASAPI", "WASAPI <http://WASAPI独占模式.com> WASAPI");

        Test("WASAPI[图片ref](文件file.assets/image.jpg)独占模式", "WASAPI[图片ref](文件file.assets/image.jpg)独占模式");

        Test("独占模式`WASAPI独占模式`独占模式", "独占模式 `WASAPI独占模式` 独占模式");
        Test("WASAPI`WASAPI独占模式`WASAPI", "WASAPI `WASAPI独占模式` WASAPI");
        Test("独占模式，`WASAPI独占模式`,WASAPI", "独占模式，`WASAPI独占模式`,WASAPI");

        Test("\"WASAPI\"独占模式", "\"WASAPI\"独占模式");
        Test("WASAPI\"独占模式\"", "WASAPI\"独占模式\"");

        Test("**WASAPI**独占模式", "**WASAPI**独占模式");
        Test("**WASAPI**独占模式", "**WASAPI**独占模式");

        Test("WASAPI.独占模式", "WASAPI.独占模式");
        Test("独占模式,WASAPI", "独占模式,WASAPI");
        Test("独占模式;WASAPI", "独占模式;WASAPI");
        Test("独占模式:WASAPI", "独占模式:WASAPI");
        Test("独占模式!WASAPI", "独占模式!WASAPI");
        Test("独占模式?WASAPI", "独占模式?WASAPI");

        Test("独占模式7.2% WASAPI", "独占模式 7.2% WASAPI");
        Test("独占模式$7 WASAPI", "独占模式 $7 WASAPI");
        Test("独占模式3-7% WASAPI", "独占模式 3-7% WASAPI");
    }
}
