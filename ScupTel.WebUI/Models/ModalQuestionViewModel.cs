namespace ScupTel.WebUI.Models
{
    public class ModalQuestionViewModel
    {

        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string YesButtonAction { get; set; }
        public string NoButtonAction { get; set; }
        public bool CloseNoButton { get; set; }
        public bool CloseYesButton { get; set; }


        public string KeyProperties { get; set; }

        public string YesButtonUpdateContainerId { get; set; }
        public string NoButtonUpdateContainerId { get; set; }
        public NotificationViewModel NoButtonNotification { get; set; }
        public NotificationViewModel YesButtonNotification { get; set; }
    }
}
