using System.Windows.Forms;

public class NoFocusButton : Button
{
    public NoFocusButton()
    {
        this.SetStyle(ControlStyles.Selectable, false);
    }

    protected override bool ShowFocusCues => false;
}