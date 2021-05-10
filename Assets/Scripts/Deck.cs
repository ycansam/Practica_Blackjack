using UnityEngine;
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

    private bool gameStop = false;  

       
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
        }
        finalMessage.text = "";
        
        if(player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21 ){
            finalMessage.text += "Empate";
            gameStop = true;
        }else if(dealer.GetComponent<CardHand>().points > 21){
            gameStop = true;
            finalMessage.text += "Jugador Gana";
        }else if(player.GetComponent<CardHand>().points > 21){
            finalMessage.text += "Dealer Gana";
            gameStop = true;
        }else if(player.GetComponent<CardHand>().points == 21 &&  dealer.GetComponent<CardHand>().points < 21){
            finalMessage.text += "Player Gana";
            gameStop = true;
        }else if(dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points < 21){
            finalMessage.text += "Dealer Gana";
            gameStop = true;
        }
    }

    private void CalculateProbabilities()
    {
        
        int playerPoints = player.GetComponent<CardHand>().points;
        probMessage.text = "";
        finalMessage.text = "";

        // probabilidad entre 21 y 17
        int counter = 0;
        for(int i = 0; i<13;i++){
            int value = i+1;
            if(i > 10){
                value = 10;
            }
            if(playerPoints+value >= 17 && playerPoints+value <= 21){
                counter++;
            }
        }
        float probabilidadEntre21 = (float)counter/(float)13;
        probMessage.text += "Prob entre 17 y 21: "+ probabilidadEntre21 + " %" + "\n";
        // Probabilidad de pasarse
        counter = 0;
        for(int i = 0; i<13;i++){
            int value = i+1;
            if(i > 10){
                value = 10;
            }
            if(playerPoints+value>21){
                counter++;
                 Debug.Log(counter);
            }
        }
        float probabilidadDePasarse = (float)counter/(float)13;
        probMessage.text +="Prob de pasarse de 21: " + probabilidadDePasarse +"%"+ "\n";

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
        if(!gameStop){
            finalMessage.text = "";
            if(player.GetComponent<CardHand>().cards.Count == 2)
            {
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            }
            
            //Repartimos carta al jugador
            PushPlayer();

            if(player.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "Player pierde";
                gameStop = true;
            }
        }
        
        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
        */
        if(player.GetComponent<CardHand>().cards.Count == 2)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        }
        finalMessage.text = "";

        bool dealerStand = false;
        while(dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();

            if(dealer.GetComponent<CardHand>().points >= 17)
            {
                dealerStand = true;
                if(dealerStand)
                {
                    if(player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21 ){
                        finalMessage.text = "Empate";
                        gameStop = true;
                    }else if(dealer.GetComponent<CardHand>().points > 21){
                        finalMessage.text = "Jugador Gana";
                        gameStop = true;
                    }else if(player.GetComponent<CardHand>().points > 21){
                        finalMessage.text = "Dealer Gana";
                        gameStop = true;
                    }else if(player.GetComponent<CardHand>().points == 21 &&  dealer.GetComponent<CardHand>().points < 21){
                        finalMessage.text = "Player Gana";
                        gameStop = true;
                    }else if(dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points < 21){
                        finalMessage.text = "Dealer Gana";
                        gameStop = true;
                    }else if(dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points){
                        finalMessage.text = "Player Gana";
                        gameStop = true;
                    }else if(dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points){
                        finalMessage.text = "Dealer Gana";
                        gameStop = true;
                    }
                }
            }
        }
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
