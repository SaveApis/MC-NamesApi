namespace RestApi.Data.Models.Translation;

/// <summary>
/// Specify the LanguageTranslation
/// </summary>
public class Translation
{
    /// <summary>
    /// Specify the Message For the AgreementController if no entry is in the database
    /// </summary>
    public string ControllerAgreementNoEntry { get; set; } = "Kein Eintrag gefunden!";

    /// <summary>
    /// Specify the message for the AgreementController if an entry is set in the database.
    /// </summary>
    public string ControllerAgreementEntryFound { get; set; } = "Eintrag wurde gefunden!";

    /// <summary>
    /// Specify the message for the AgreementController if someone try to set the same value in the Database.
    /// </summary>
    public string ControllerAgreementNoChanges { get; set; } =
        "Derselbe Eintrag wurde in der Datenbank gefunden! Nichts wurde verändert.";
}