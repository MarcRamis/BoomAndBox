public class Combo
{
    private int comboCounter = 0;
    private int maxCombo = 0;
    
    public int GetComboCounter() { return comboCounter; }
    public void SetMaxCombo(int _maxCombo) { maxCombo = _maxCombo; }
    public void SumCombo() { comboCounter++; }
    public void ComboFailed() { comboCounter = 0; }
    public bool ComboAccomplished()
    {
        if (comboCounter >= maxCombo)
        {
            comboCounter = 0;
            return true;
        }

        return false;
    }
}