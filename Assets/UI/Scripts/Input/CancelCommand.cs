namespace UI.Scripts.Input
{
    public class CancelCommand : Command
    {
        public override void Execute()
        {
            UIManager.Instance.PressCancel();
        }
    }
}