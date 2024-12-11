namespace IoT_App.Models.Store
{
    public class UserSettings
    {
        public bool IsProtected { get; set; } = false;

        public AutoBehaviourSettings AutoBehaviourSettings { get; set; } = new AutoBehaviourSettings();
    }
}
