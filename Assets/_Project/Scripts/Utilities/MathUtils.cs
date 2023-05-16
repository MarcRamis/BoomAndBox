using UnityEngine;

public static class MathUtils
{
    // Clase estática Randomize
    public static class Randomize
    {
        // Método Get que devuelve un elemento aleatorio del arreglo
        public static Transform Get(Transform[] array)
        {
            // Genera un índice aleatorio dentro del rango del arreglo
            int randomIndex = Random.Range(0, array.Length);
            // Devuelve el elemento del arreglo correspondiente al índice generado
            return array[randomIndex];
        }

        // Método GetRandomTarget que devuelve el primer elemento aleatorio del arreglo mezclado aleatoriamente
        public static Transform GetRandomTarget(Transform[] array)
        {
            // Mezcla aleatoriamente los elementos del arreglo
            Transform[] randomizedTargets = RandomizeArray(array);
            // Devuelve el primer elemento del arreglo resultante
            return randomizedTargets[0];
        }

        // Método RandomizeArray que mezcla aleatoriamente los elementos del arreglo
        public static Transform[] RandomizeArray(Transform[] array)
        {
            // Bucle que recorre cada elemento del arreglo
            for (int i = 0; i < array.Length - 1; i++)
            {
                // Genera un índice aleatorio dentro del rango restante del arreglo
                int randomIndex = Random.Range(i, array.Length);
                // Intercambia el elemento actual con el elemento aleatorio generado
                Transform temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
            // Devuelve el arreglo mezclado aleatoriamente
            return array;
        }
    }
}