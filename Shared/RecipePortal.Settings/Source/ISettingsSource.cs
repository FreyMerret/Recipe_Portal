namespace RecipePortal.Settings;

public interface ISettingsSource
{
    string GetConnectionString(string source = null);
    string GetAsString(string source, string defaultValue = null);
    bool GetAsBool(string source, bool defaultValue = false);
    int GetAsInt(string source, int defaultValue = 0);
}
