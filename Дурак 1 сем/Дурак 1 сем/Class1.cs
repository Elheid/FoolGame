using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Дурак_1_сем;

namespace Дурак_1_сем
{
     public static class ListExstensions
     {
         public static void MoveSomeEl(this List<Card> list, int oldIndex, int newIndex)//перемещает что-то в списке с одного места на другое
         {
             var item = list[oldIndex];
             list.RemoveAt(oldIndex);
             if (newIndex > oldIndex) newIndex--;
             list.Insert(newIndex, item);
         }
     }

     public class Game
     {
        public enum PlayerCommands
        {
            CheckTrumpMark, /*= Move.GetTrumpCard(),*/
            CheckMyCardDeck,
            CheckCardsOnTable,
            AttackWithCard,
            ThrowOneMoreCard,
            TakeAllCardsFromTable,
            BeatCardOnTableWithThis,
            TurnEnd,
            CheckPossibleCommands,
        }

        public PlayerCommands Command;

        public Game(PlayerCommands commands)
        {
            Command = commands;
        }

        public static bool TurnIsEnd;
        public static bool ContinueTurn;
        public static Player CreatePlayer()
        {
            var player = new Player();//создаёт игрока

            player = Move.InitializePlayer();//запоминает имя (1) игрока выводит строки в консоль
            Move.DistribuuteCardToPlayer(player);//раздаёт карты из колоды в руку игрока
          
            return player;
        }

        public void StartMove(Player player)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Ход игрока " + player);
            player.CanMove = true;
        }

        public static void ShowPlayerCommands()//показывает все возможные/доступные для игрока команды
        {
            Console.WriteLine("\n");
            for (var i = 0; i <= (int)PlayerCommands.CheckPossibleCommands; i++)
            {
                Console.WriteLine(i + " " + (PlayerCommands)i);
            }
        }

        public static void DoPlayerCommand(Player player)
        {
            if (ContinueTurn)
            {
                Console.WriteLine("Введите номер карты, чтобы подкинуть");
                var cardToPut = Console.ReadLine();
                player.CurrentMove.ThrowOneMoreCard(player, int.Parse(cardToPut));
                return;
            }


            Console.WriteLine("\n");
            Console.WriteLine("Введите номер команды");
            var curCommand = Console.ReadLine();
            var command = (PlayerCommands)int.Parse(curCommand);
            switch (command)
            {
                case PlayerCommands.CheckTrumpMark:
                    Move.GetTrumpCard();
                    break;
                case PlayerCommands.CheckMyCardDeck:
                    player.CurrentMove.ShowCurrentDeck(player);
                    break;
                case PlayerCommands.CheckCardsOnTable:
                    player.CurrentMove.ShowCurrentDeck(Table.CardsOnTable);
                    break;
                case PlayerCommands.AttackWithCard:
                    Console.WriteLine("Введите номер карты, чтобы положить на стол");
                    var cardToPutToAttack = Console.ReadLine();
                    player.CurrentMove.PutCardOnTable(player, int.Parse(cardToPutToAttack));
                    break;
                case PlayerCommands.ThrowOneMoreCard:
                    Console.WriteLine("Введите номер карты, чтобы подкинуть");
                    var cardToPutToThrow = Console.ReadLine();
                    player.CurrentMove.ThrowOneMoreCard(player, int.Parse(cardToPutToThrow));
                    break;
                case PlayerCommands.TakeAllCardsFromTable:
                    player.CurrentMove.TakeCardsFromTable(player.PlayerDeck);
                    break;
                case PlayerCommands.BeatCardOnTableWithThis:
                    Console.WriteLine("Введите 2 числа номер карты из своей колоды и карту, которую хотите покрыть");
                    var cardOfPlayer = int.Parse(Console.ReadLine());
                    var cardOnTable = int.Parse(Console.ReadLine());
                    Move.BeatCardWithThis(player, cardOfPlayer, cardOnTable);
                    break;
                case PlayerCommands.TurnEnd:
                    Console.WriteLine("Игрок " + player.Name + " решил закончить ход");
                    TurnIsEnd = true;
                    break;
                case PlayerCommands.CheckPossibleCommands:
                    ShowPlayerCommands();
                    break;
            }
        }

     }

     public class Card
     {
        public enum ScoreOfCard
        {
            two = 1,
            three,
            four,
            five,
            six,
            seven,
            eight,
            nine,
            ten,
            jack,
            queen,
            king,
            ace
        }

        public enum TypeOfMark
        {
            hearts = 1,
            diamonds,
            spades,
            clubs,
        }
        public TypeOfMark Mark;
        public ScoreOfCard Score;

        public Card (TypeOfMark mark, ScoreOfCard score)
        {
            Mark = mark;
            Score = score;
        }
     }

     public class Player
     {
         public bool CanMove;
         public CardDeck PlayerDeck;
         public Move CurrentMove = new Move();
         private string name;
         public string Name 
         { 
            get { return name; }//доделать
            set
             {
                    //var nameInCorrect = true;
                    ////while (nameInCorrect)
                    //{
                    //    if (value == null || value == "" || value == "\n")
                    //    {
                    //        Console.WriteLine("Некорректное имя, введите ещё раз");
                    //        ////Player.NameAsk(Game.Player1);
                    //        nameInCorrect = true;
                    //        continue;
                    //    }
                    //    else
                    //    nameInCorrect = false;
                    //}
                 name = value;
            } 
         }

        private static void NameAsk(Player currentPlater)//запоминает имя игрока
        {
            Console.WriteLine("\n");
            Console.WriteLine("Введите имя игрока");
            currentPlater.Name = Console.ReadLine();
        }

     }
     
     public class Move
     {

        public static void DistribuuteCardToPlayer(Player player)//раздает карты игроку в начале
        {
            var playerDeck = new CardDeck();
            player.PlayerDeck = playerDeck;
            var rnd = new Random();
            var j = rnd.Next(2);
            var tableDeck = Table.CardDeck.CardsInDeck;
            var countCardsInDeck = 6;
            for (int i = 0; i < countCardsInDeck; i++)
            {
                if (j + i > tableDeck.Count)
                    continue;
                playerDeck.CardsInDeck.Add(tableDeck[j + i]);
                tableDeck.Remove(tableDeck[j + i]);
            }
        }

        public static Player InitializePlayer()
        {
            Console.WriteLine("\n");
            var player = new Player();
            Console.WriteLine("Введите имя игрока");
            player.Name = Console.ReadLine();
            return player;
        }

         public static Card TakeCard(CardDeck deck, int numberOfCard)//возвращает карту из колоды
         {
            return deck.CardsInDeck[numberOfCard];
         }

         public static void PlayerGetMissingCards(Player player)//позволяет добрать недостающие карты в руки
         {
            var maxCountOfCard = 6;
            var lastCard = Table.CardDeck.CardsInDeck.Count - 1;
            if (player.PlayerDeck.CardsInDeck.Count < 6)
            {
                for (var i = 0; i < maxCountOfCard - player.PlayerDeck.CardsInDeck.Count; i++)
                {
                    player.PlayerDeck.CardsInDeck.Add(TakeCard(Table.CardDeck, lastCard - i));
                    Table.CardDeck.CardsInDeck.RemoveAt(lastCard - i);
                }
            }
         }

         public static void GetTrumpCard()//позволяет увидет козырную масть
         {
            Console.WriteLine("\n");
            Console.WriteLine( "Козырная карта = " +  CardDeck.TrumpMark);
         }

         public void ShowCurrentDeck(CardDeck deck)//позваляет увидет колоду
         {
            foreach (var card in deck.CardsInDeck)
            {
                Console.Write(deck.CardsInDeck.IndexOf(card) + " Карта " + card.Mark + " " + card.Score);
                Console.WriteLine();
            }
         }

        public void ShowCurrentDeck(Player player)//более удобно выводит колоду опред. игрока
        {
            Console.WriteLine("\n");
            Console.WriteLine("Колода " + player.Name + "\n");
            for (var i = 0; i < player.PlayerDeck.CardsInDeck.Count; i++)
            {
                Console.Write(i + " " + "Карта " 
                    + player.PlayerDeck.CardsInDeck[i].Mark
                    + " " + player.PlayerDeck.CardsInDeck[i].Score);
                Console.WriteLine();
            }
            Console.WriteLine("\n");
        }

        public static bool CardCompare(Card cardOnTable, Card cardOfPlayer)// можно ли картой покрыть другую карту
        {
            var suitsEqual = cardOnTable.Mark== cardOfPlayer.Mark;
            var playerCardScoreOver = cardOnTable.Score < cardOfPlayer.Score;
            var cardOnTableTrump = cardOnTable.Mark == CardDeck.TrumpMark;
            var cardOfPlayerTrump = cardOfPlayer.Mark == CardDeck.TrumpMark;
            if (cardOfPlayerTrump && !suitsEqual)
            {
                return true;
            }
            else if (cardOnTableTrump && suitsEqual && playerCardScoreOver)
            {
                return true;
            }
            else if (suitsEqual && playerCardScoreOver)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Ход невозможен");
                return false;
            }
        }

        public static void BeatCardWithThis(Player player, int cardOfPlayer, int cardOnTable)//позволяет попробовать побить карту
        {
            if (player.PlayerDeck.CardsInDeck.Count < cardOfPlayer || Table.CardsOnTable.CardsInDeck.Count < cardOnTable)
            {
                Console.WriteLine("Неправильно набраны номера, карт с таким номером в одной/обеих колод/е/ах не найдено");
                return;
            }
            var playerCard = player.PlayerDeck.CardsInDeck[cardOfPlayer];
            var tableCard = Table.CardsOnTable.CardsInDeck[cardOnTable];
            if (CardCompare(playerCard, tableCard))
            {
                Table.BeatenCards.Add(playerCard);
                Table.BeatenCards.Add(tableCard);
            }
            else
            {
                Console.WriteLine("Покрыть карту картой не удалось, возможно вы ошиблись номером?");
                return;
            }
        }

        public void PutCardOnTable(Player player, int numberOfCard)//кладёт карту на стол
        {
            var nameOfCard = " " + player.PlayerDeck.CardsInDeck[numberOfCard].Mark
                + " " + player.PlayerDeck.CardsInDeck[numberOfCard].Score;
            if (player.CanMove == false)
            {
                Console.WriteLine("Это не ваш ход");
                return;
            }
            Console.WriteLine("\n");
            Console.WriteLine("Игрок " + player.Name + " положил на стол карту" + nameOfCard);
            Table.CardsOnTable.CardsInDeck.Add(TakeCard(player.PlayerDeck, numberOfCard));
            player.PlayerDeck.CardsInDeck.RemoveAt(numberOfCard);
        }

        public void ThrowOneMoreCard(Player player,int cardOfPlayerToAdd)//подкидывает карту на стол, если есть одинаковые значения
        {
            var playerCard = player.PlayerDeck.CardsInDeck[cardOfPlayerToAdd];
            if (Table.CardsOnTable.CardsInDeck.Count == 0)
            {
                Console.WriteLine("На столе нет карт, подкинуть карту нельзя");
                return;
            }
            for (var i =0; i <= Table.CardsOnTable.CardsInDeck.Count; i++)
            {
                if (playerCard.Score == Table.CardsOnTable.CardsInDeck[i].Score)
                {
                    Console.WriteLine(player.Name + " Подкинул карту " + playerCard.Mark + " "+ playerCard.Mark);
                    PutCardOnTable( player, cardOfPlayerToAdd);
                }
                break;
            }
        }

        public void AddAllCoveredCardsToTrash(Trash trash)//победа защиты, всё в биту
        {
            foreach(var card in Table.BeatenCards)
            {
                trash.MoveToTrash(Table.BeatenCards, card);
                Table.BeatenCards.Remove(card);
            }
        }

        public void TakeCardsFromTable(CardDeck deckToPut)//победа атаки, взять все карты
        {
            if (Table.CardsOnTable.CardsInDeck.Count == 0)
            {
                Console.WriteLine("На столе нет карт");
                return ;
            }
            for (var i = 0; i < Table.CardsOnTable.CardsInDeck.Count; i++)
            {
                deckToPut.CardsInDeck.Add(TakeCard(Table.CardsOnTable, i));
                Table.CardsOnTable.CardsInDeck.RemoveAt(i);
                Console.WriteLine("Игрок взял все карты со стола");
            }
        }
     }

     class Rules
     {
        public bool CheckAllRulesFollowed()
        {
            throw new ArgumentException();
        }
        public bool CheckMovePossibility()
        {
               //корректный или некорректный ход
            throw new ArgumentException();
        }
        public Player GetCurrentPlayer()
        { //что делает конкретный игрок, его ли ход
               throw new ArgumentException();
        }
           //singleTon
     }

      public static class Table
      {
         public static Move CurrentMove = new Move();
         public static CardDeck CardDeck = new CardDeck();
         public static Trash Trash = new Trash();
         public static CardDeck CardsOnTable = new CardDeck();
         public static List<Card> BeatenCards = new List<Card>();

         public static void PrepareDeck()//подготавливает колоду
         {
            Table.CardDeck.CreateTableDeck();//создает колоду 52 карты
            Table.CardDeck.MixDeck();//миксует колоду случайно
            Table.CardDeck.AssignTrump();//находит рандомную карту - козырку, перемещает её в начало колоды
         }
      }

     public class Trash///бита
     {
         public CardDeck _Trash = new CardDeck();
         //сюда все битые карты их нельзя видеть/трогать
         public void MoveToTrash(List<Card> deck, Card card)
         {
                _Trash.CardsInDeck.Add(card);
                deck.Remove(card);
         }
     }
    public class CardDeck
    {
        public List<Card> CardsInDeck = new List<Card>();
        public static Card.TypeOfMark TrumpMark;

        public void CreateTableDeck()//созает колоду 52
        {
            for (int i = 1; i <= (int)Card.TypeOfMark.clubs; i++)
            {
                for (int j = 1; j <= (int)Card.ScoreOfCard.ace; j++)
                {
                    var card = new Card((Card.TypeOfMark)i, (Card.ScoreOfCard)j);
                    CardsInDeck.Add(card);
                }
            }
        }

        public void MixDeck()//перемешывает
        {
            var random = new Random();
            for (int i = 0; i < CardsInDeck.Count; i++)
            {
                var j = random.Next(i + 1);
                var temp = CardsInDeck[j];
                CardsInDeck[j] = CardsInDeck[i];
                CardsInDeck[i] = temp;
            }
        }

        public void AssignTrump()//выбирает козырку, перемещает в начало
        {
            var rnd = new Random();
            var random = rnd.Next(CardsInDeck.Count);
            var rndCard = CardsInDeck[random];
            TrumpMark = rndCard.Mark;
            CardsInDeck.MoveSomeEl(random, 0);
        }
    }
}


