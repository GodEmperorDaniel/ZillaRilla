namespace UI.Scripts.Input
{
    public class AcceptCommand : Command
    {
        public override void Execute()
        {
            UIManager.Instance.PressAccept();
        }
    }
}