
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;



        // Ваши сохранения
        public int[] completedLevelsStars = new int[130];
        public int completedLevels = 1;
        public bool sounds = true;
        public bool music = true;
        // ...
    }
}
