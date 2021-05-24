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
    public Text probMessage1;
    public Text probMessage2;

    public int[] values = new int[52];
    int cardIndex = 0;  

    // mio
    private bool gameStop = false;  
    private int banca = 1000;
    private int apuesta = 0;
    public Text Banca;
    public Text Apuesta;
    private bool partidaEmpezada = false; // si la partida no ha empezado puede apostar

       
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
            apuesta = 0;
            gameStop = true;
            Banca.text = "Banca: " + banca.ToString();
            Apuesta.text = "Apuesta: " + apuesta.ToString();
        }else if(dealer.GetComponent<CardHand>().points > 21){
            gameStop = true;
            finalMessage.text += "Jugador Gana";
            banca += apuesta*2;
            apuesta = 0;
            Banca.text = "Banca: " + banca.ToString();
            Apuesta.text = "Apuesta: " + apuesta.ToString();
        }else if(player.GetComponent<CardHand>().points > 21){
            finalMessage.text += "Dealer Gana";
            apuesta = 0;
            Banca.text = "Banca: " + banca.ToString();
            Apuesta.text = "Apuesta: " + apuesta.ToString();
            gameStop = true;
        }else if(player.GetComponent<CardHand>().points == 21 &&  dealer.GetComponent<CardHand>().points < 21){
            finalMessage.text += "Player Gana";
            gameStop = true;
            banca += apuesta*2;
            apuesta = 0;
            Banca.text = "Banca: " + banca.ToString();
            Apuesta.text = "Apuesta: " + apuesta.ToString();
        }else if(dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points < 21){
            finalMessage.text += "Dealer Gana";
            apuesta = 0;
            Banca.text = "Banca: " + banca.ToString();
            Apuesta.text = "Apuesta: " + apuesta.ToString();
            gameStop = true;
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

        int playerPoints = player.GetComponent<CardHand>().points;
        probMessage.text = "";
        probMessage1.text = "";
        probMessage2.text = "";
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
        probMessage1.text += probabilidadEntre21*100 +" % 17-21";

        // Probabilidad de pasarse
        counter = 0;
        for(int i = 0; i<13;i++){
            int value = i+1;
            if(i > 10){
                value = 10;
            }
            if(playerPoints+value>21){
                counter++;
            }
        }
        float probabilidadDePasarse = (float)counter/(float)13;
        probMessage2.text += probabilidadDePasarse*100 +" % Pasarse";

        Debug.Log(dealer.GetComponent<CardHand>().points);

        if(cardIndex >=3){
            int dealerPoints = dealer.GetComponent<CardHand>().points;
            // Probabilidad de que el dealer tenga mas puntuacion que el jugador
            counter = 0;
            for(int i = 0; i<13;i++){
                int value = i+1;
                if(i > 10){
                    value = 10;
                }
                if(dealerPoints+value-dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().value > playerPoints){
                    counter++;
                }
            }
            float probabilidadDealerJugador = (float)counter/(float)13;
            probMessage.text += probabilidadDealerJugador*100 +" % dealer mas que jugador";   
        }
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;
        CalculateProbabilities();
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
            partidaEmpezada = true; //para impedir apostar
            finalMessage.text = "";
            //Repartimos carta al jugador
            PushPlayer();

            if(player.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "Player pierde";
                apuesta = 0;
                Banca.text = "Banca: " + banca.ToString();
                Apuesta.text = "Apuesta: " + apuesta.ToString();
                gameStop = true;
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
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
        if(!gameStop){
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            partidaEmpezada = true; // para impedir apostar
            if(player.GetComponent<CardHand>().cards.Count == 2)
            {
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            }
            finalMessage.text = "";

            bool dealerStand = false;
            while(dealer.GetComponent<CardHand>().points <= 16)
            {
                PushDealer();
            }
            if(dealer.GetComponent<CardHand>().points > 16)
                {
                    dealerStand = true;
                    if(dealerStand)
                    {
                        if(player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21 ){
                            finalMessage.text = "Empate";
                            apuesta = 0;
                            Banca.text = "Banca: " + banca.ToString();
                            Apuesta.text = "Apuesta: " + apuesta.ToString();
                            gameStop = true;
                        }else if(dealer.GetComponent<CardHand>().points > 21){
                            finalMessage.text = "Jugador Gana";
                            banca += apuesta*2;
                            apuesta = 0;
                            Banca.text = "Banca: " + banca.ToString();
                            Apuesta.text = "Apuesta: " + apuesta.ToString();
                            gameStop = true;
                        }else if(player.GetComponent<CardHand>().points > 21){
                            finalMessage.text = "Dealer Gana";
                            apuesta = 0;
                            Banca.text = "Banca: " + banca.ToString();
                            Apuesta.text = "Apuesta: " + apuesta.ToString();
                            gameStop = true;
                        }else if(player.GetComponent<CardHand>().points == 21 &&  dealer.GetComponent<CardHand>().points < 21){
                            finalMessage.text = "Player Gana";
                            banca += apuesta*2;
                            apuesta = 0;
                            Banca.text = "Banca: " + banca.ToString();
                            Apuesta.text = "Apuesta: " + apuesta.ToString();
                            gameStop = true;
                        }else if(dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points < 21){
                            finalMessage.text = "Dealer Gana";
                            apuesta = 0;
                            Banca.text = "Banca: " + banca.ToString();
                            Apuesta.text = "Apuesta: " + apuesta.ToString();
                            gameStop = true;
                        }else if(dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points){
                            finalMessage.text = "Player Gana";
                            banca += apuesta*2;
                            apuesta = 0;
                            Banca.text = "Banca: " + banca.ToString();
                            Apuesta.text = "Apuesta: " + apuesta.ToString();
                            gameStop = true;
                        }else if(dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points){
                            finalMessage.text = "Dealer Gana";
                            apuesta = 0;
                            Banca.text = "Banca: " + banca.ToString();
                            Apuesta.text = "Apuesta: " + apuesta.ToString();
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
        if(banca >= 0){
            gameStop = false;
            partidaEmpezada = false;
            apuesta = 0;
            Banca.text = "Banca: " + banca.ToString();
            Apuesta.text = "Apuesta: " + apuesta.ToString();
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

    public void SumarApuesta(){
        if(!partidaEmpezada){
            if(banca > 0){
                apuesta += 10;
                banca -= 10;
                Banca.text = "Banca: " + banca.ToString();
                Apuesta.text ="Apuesta: " + apuesta.ToString();
            }
        }
    }
    public void RestarApuesta()
    {
        if(!partidaEmpezada){
            if(apuesta > 0){
                apuesta -= 10;
                banca += 10;
                Banca.text = "Banca: " + banca.ToString();
                Apuesta.text = "Apuesta: " + apuesta.ToString();
            }
        }
    }
}
