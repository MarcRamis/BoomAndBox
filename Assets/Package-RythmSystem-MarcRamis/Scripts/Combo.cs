
// Esta clase Combo define el comportamiento del sistema de combos en un videojuego.
using System;

public class Combo
{
    // Variables de la clase
    private int comboCounter = 0; // contador de combo actual
    private int maxCombo = 0; // contador máximo de combo posible

    // Método que devuelve el contador de combo actual
    public int GetComboCounter() { return comboCounter; }

    // Método que establece el contador máximo de combo posible
    public void SetMaxCombo(int _maxCombo) { maxCombo = _maxCombo; }
    
    // Método que suma uno al contador de combo actual
    public void SumCombo() { if (comboCounter < maxCombo) comboCounter++; }
    public void SumCombo(int sumCounter) { if (comboCounter < maxCombo) comboCounter += sumCounter; }

    // Método que reinicia el contador de combo actual
    public void ComboFailed() { comboCounter = 0; }

    // Método que comprueba si se ha alcanzado el contador máximo de combo posible
    // Si es así, reinicia el contador actual y devuelve true, sino devuelve false
    public bool ComboAccomplished()
    {
        if (comboCounter >= maxCombo)
        {
            comboCounter = 0;
            return true;
        }

        return false;
    }

    public void Rest()
    {
        comboCounter--;
    }
}