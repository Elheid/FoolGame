using Дурак_1_сем;

namespace Fool
{

    public class Programm
    {

        public static void Main(string[] args)
        {
            ////просто проверка функций
            //var trash = Table.Trash = new Trash();
            //var tableDeck = Table.CardDeck;
            //var tableMove = Table.CurrentMove = new Move();
            //Move.PrepareDeck(tableDeck);//создает колоду и миксует её, находит козырку и перемещает её в начало
            //tableMove.ShowCurrentDeck(tableDeck);//выводит все карты колоды в консоль

            //var player1 = Game.CreatePlayer();
            //var player2 = Game.CreatePlayer();

            //tableMove.ShowCurrentDeck(player1);//выводит все карты колоды в консоль
            //tableMove.ShowCurrentDeck(player2);//выводит все карты колоды в консоль

            //Move.GetTrumpCard();//выводит козырку

            //player1.CurrentMove.PutCardOnTable(player1, 1);// игрок кладёт карту на стол
            //Table.CurrentMove.ShowCurrentDeck(Table.CardsOnTable);//выводит все карты колоды в консоль



            ////player2.CurrentMove.TakeCardsFromTable(table.CardsOnTable, player2.PlayerDeck);


            ////tableMove.ShowCurrentDeck(tableDeck);//выводит все карты колоды в консоль

            //tableMove.ShowCurrentDeck(player1);//выводит все карты колоды в консоль
            //tableMove.ShowCurrentDeck(player2);
            //Move.PlayerGetMissingCards(player1);//игрок добирает недостоющие карты

            //player1.CurrentMove.ThrowOneMoreCard(player1, 2);
            //tableMove.ShowCurrentDeck(Table.CardsOnTable);
            ////player1.CurrentMove.BeatCardWithThis(table, player1, 1, 1);

            //player2.CurrentMove.ShowCurrentDeck(player2);
            //player2.CurrentMove.ShowCurrentDeck(trash._Trash);
            ////
            ///
            Console.WriteLine("Запуск игры : Дурак, стандарт");
            Table.PrepareDeck();
            //Table.CurrentMove.ShowCurrentDeck(Table.CardDeck);
            var player1 = Game.CreatePlayer();
            var player2 = Game.CreatePlayer();
            Game.ShowPlayerCommands();
            var motionCount = 0;
            while (true)
            {
                motionCount++;
                if (motionCount % 2 != 0)
                {
                    player1.CanMove = true;
                    player2.CanMove = false;
                }
                else
                {
                    player1.CanMove = false;
                    player2.CanMove = true;
                }
                var curPlayer = player1.CanMove ? player1 : player2;
                var otherPlayer = player1.CanMove ? player2 : player1;

                Move.PlayerGetMissingCards(player1);
                Move.PlayerGetMissingCards(player2);

                Console.WriteLine("Ход " + motionCount + " " + curPlayer.Name);
                Game.TurnIsEnd = false;
                while (!Game.TurnIsEnd)
                    Game.DoPlayerCommand(curPlayer);
                
                if (Game.TurnIsEnd)
                {
                    if (motionCount != 1)
                    {

                        Console.WriteLine("Хочет ли игрок " + otherPlayer.Name + "  подкинуть карты?");
                        Console.WriteLine("0 Да");
                        Console.WriteLine("1 Нет");
                        var continueAnswer = Console.ReadLine();
                        Game.ContinueTurn = continueAnswer == "0" ? true : false;
                        if (Game.ContinueTurn)
                            Game.DoPlayerCommand(otherPlayer);
                    }
                    while (Game.ContinueTurn)
                    {
                        Console.WriteLine("Ещё?");
                        Console.WriteLine("0 Да");
                        Console.WriteLine("1 Нет");
                        var continueAnswer = Console.ReadLine();
                        Game.ContinueTurn = continueAnswer == "0" ? true : false;
                        if (Game.ContinueTurn)
                            Game.DoPlayerCommand(otherPlayer);
                        else
                            break;
                    }
                    if (Game.TurnIsEnd)
                    {
                        Game.TurnIsEnd = false;
                        continue;
                    }
                }

                var gameIsEnd = player1.PlayerDeck.CardsInDeck.Count == 0
                || player2.PlayerDeck.CardsInDeck.Count == 0
                || Table.CardDeck.CardsInDeck.Count == 0;//закончились карты у игрока и в колоде
                if (gameIsEnd)
                {
                    break;
                }
                Console.WriteLine();
                Console.ReadKey();
            }
        }
    }
}