namespace Notes.Tests.Helpers
{
    public static class Consts
    {
        public static string nonexistentNoteUrl = "/api/notes/20";
        public static string deletedNoteUrl = "/api/notes/3";
        public static string properIdUrl = "api/notes/1";
        public static string properHistoryIdUrl = "api/notes/history/1";
        public static string nonexistentNoteHistoryUrl = "api/notes/history/20";
        public static string postUrl = "/api/notes";
        public static int newNoteId = 5;
        public static int originalNoteId = 1;
        public static int numberOfNotesInHistory = 2;
        public static int nonexistentNoteId = 20;
        public static int deletedNoteId = 3;
    }
}
