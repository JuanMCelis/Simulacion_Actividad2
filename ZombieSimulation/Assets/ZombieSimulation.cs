using System.Collections.Generic;
using UnityEngine;

public class ZombieSimulation : MonoBehaviour //Crea un script llamado ZombieSimulation
{
    // Se declaran las variables para el ejercicio
    public GameObject personaPrefab; 
    public int sanosInicial = 100;
    public int zombisInicial = 1;
    public float tiempoPorDia = 1f;

    private List<SpriteRenderer> personas = new List<SpriteRenderer>();
    private float contadorTiempo;
    private int dia;
   // Llama a las funciones 
    void Start()
    {
        CrearPoblacion();
        InfectarIniciales();
        DrawView();
    }

    void CrearPoblacion()
    {
        int total = sanosInicial + zombisInicial; //Esto crea los 101 cuadros en pantalla

        for (int i = 0; i < total; i++) 
        {
            Vector2 pos = new Vector2((i % 11) * 1.2f, (i / 11) * 1.2f); 
            //Esta linea organiza los cuadros en la pantalla --> i%11 organiza 11 columna,
            //i/11 hace las 11 filas y 1.2f es para que haya un espacio entre columnas

            GameObject p = Instantiate(personaPrefab, pos, Quaternion.identity);
            // Primero se crea una variable p, con instantiate se clona el objeto q seria 
            // el prefab q unimos en el inspector de Unity. pos es el vector q creamos en la linea
            // anterior. Por ultimo se define sin rotacion y orientacion normal.
       
            personas.Add(p.GetComponent<SpriteRenderer>());
            // Esta linea de codigo nos permite acceder a las propiedades del prefab 
            // para que podamos cambiarle el color y tal.



        }
    }
    //Esta funcion hace q los cuadros se pongan rojos para asi representar a los zombies  
    void InfectarIniciales()
    {
        for (int i = 0; i < zombisInicial; i++)
        {
            personas[i].color = Color.red;
        }
    }

    void Update()
    {
        if (TodasInfectadas()) return; //Si no hay sanos se detiene la simulacion

        contadorTiempo += Time.deltaTime; //representa el tiempo transcurrido entre frames

        if (contadorTiempo >= tiempoPorDia) //Revisa si se cumplio el tiempo de pasar al dia siguiente
        {
            contadorTiempo = 0f;
            Simulate();
            DrawView();
        }
    }

    void Simulate()
    {
        int zombis = ContarZombis(); //cuenta cuantos zombies hay
        int sanos = personas.Count - zombis; //Cuenta las personas sanas

        if (sanos <= 0) return; //Si no hay sanos no infecat

        
        int nuevos = Mathf.Min(zombis, sanos); // Aca aplica el modelo matematico donde cada zombie infecta a 1 persona de las sanas
        // ejemplos Mathf.min (8 (zombies), 93 (sanos)) solo pueden cambiar de color 8 de los sanos
        int infectados = 0;

        for (int i = 0; i < personas.Count && infectados < nuevos; i++) //Hace el recorrido de personas sanas para ver si estan infectadas o no
        {
            if (personas[i].color != Color.red) // Si esta sana la convierte
            {
                personas[i].color = Color.red; //Convierte las personas a zombies
                infectados++; // Aumenta el contador de Zombies
            }
        }

        dia++; //Cambia de día 
    }

    //Cuenta cuantos zombies estan en rojo
    int ContarZombis()
    {
        int total = 0;
        foreach (var p in personas) //recorre cada elemento q hay en la lista
                                    //q puse personas donde var detecta el tipo automaticamente y p es solo una variable
                                    //temporal para ejecutar el recorrdio
            if (p.color == Color.red) total++; //Verifica si es zombie o no, y si es lo suma al contador
        return total; //devuelve el numero de rojos (zombies) encontrados
    }
    //Cuando todas las personas esten infectadas retornara true para detener la simulacion
    bool TodasInfectadas()
    {
        return ContarZombis() == personas.Count;
    }
    //Esta funcion muestra el estado del sistema en consola
    void DrawView()
    {
        int zombis = ContarZombis();
        int sanos = personas.Count - zombis;

        Debug.Log("Día " + dia + ": " + sanos + " sanos, " + zombis + " zombis");

        if (sanos == 0)
            Debug.Log("Todos han sido infectados. Fin de la simulación. GG WP. Se han extingido todos los seres humanos F");
    }
}