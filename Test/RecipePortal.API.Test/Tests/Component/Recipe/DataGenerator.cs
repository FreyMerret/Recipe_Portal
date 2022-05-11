namespace RecipePortal.API.Test.Tests.Component.Recipe;

public partial class RecipeIntegrationTest
{
    public static class Generator
    {
        public static string[] ValidTitles =
        {
            new string('t',1),
            new string('t',20),
            new string('t',50)
        };

        public static string[] InvalidTitles =
        {
            null,
            "",
            new string('t',51)
        };

        public static string[] ValidDescriptions =
        {
            new string('d',1),
            new string('d',200)
        };

        public static string[] InvalidDescriptions =
        {
            null,
            new string('1',201)
        };

        public static int[] ValidCategoryId =
        {
            1,
            2
        };

        public static int[] InvalidCategoryId =
        {
            0,
            -1
        };

        public static string[] ValidTexts =
        {

            new string('d',1),
            new string('d',200)
        };

        public static string[] InvalidTexts =
        {
            null,
            new string('1',2001)
        };
    }
}
