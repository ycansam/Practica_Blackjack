﻿using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {   
        int inicio = 0;
        int fin = 13;

        for(int i= 0;i<4;i++)
        {
            for(int j = inicio; j<fin;j++)
            {
                if(j != inicio && j != fin-3 && j !=fin-2 && j != fin-1)
                {
                    values[j] = j+1-inicio;
                }else if(j == inicio){
                    values[j] = 1;
                }else if(j-inicio == fin-3-inicio || j-inicio == fin-2-inicio || j-inicio == fin-1-inicio){
                    values[j] = 10;
                }
            }
            inicio += 13;
            fin +=13;
        }
    }

    private void ShuffleCards()
    {
        Sprite[] facesAux = new Sprite[52];
        int[] valuesAux = new int[52];

        facesAux = faces;
        valuesAux = values;

        int n = values.Length;
        while (n>1){
            n--;
            int k = Random.Range(0,n+1);
            var face = facesAux[k];
            facesAux[k] = facesAux[n];
            facesAux[n] = face;

            var value = valuesAux[k];
            valuesAux[k] = valuesAux[n];
            valuesAux[n] = value;
        }

        faces = facesAux;
        values = valuesAux;
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();

            if(player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21 ){
                Debug.Log("Empate");
            }else if(player.GetComponent<CardHand>().points > 21 && dealer.GetComponent<CardHand>().points > 21 ){
                Debug.Log("Los dos pierden");
            }else if(player.GetComponent<CardHand>().points > 21 && dealer.GetComponent<CardHand>().points <= 21 ){
                Debug.Log("El dealer gana, player superó 21");
            }else if(player.GetComponent<CardHand>().points <= 21 && dealer.GetComponent<CardHand>().points > 21 ){
                Debug.Log("El jugador gana, dealer superó 21");
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
