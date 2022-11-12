using MineField.Model;
using MineField.Model.Cells;
using MineField.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineField.Persistence
{
    public class MineFieldPersistence
    {
        public async Task SaveGame(string filePath, MineFieldState gameToSave)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(gameToSave.BoardSize + ";");
                    writer.Write(gameToSave.BombCount + ";");
                    await writer.WriteLineAsync(gameToSave.RevealedCells.ToString());
                    writer.Write(gameToSave.FirstPlayerIsNext);
                    await writer.WriteLineAsync();
                    var boardRange = Enumerable.Range(0, gameToSave.BoardSize);
                    foreach (int verticalPosition in boardRange)
                    {
                        foreach (int horizontalPosition in boardRange)
                        {
                            await writer.WriteLineAsync(gameToSave[verticalPosition, horizontalPosition].ToString());
                        }
                    }
                }
            }
            catch
            {
                throw new MineFieldDataException();
            }
        }

        public async Task<MineFieldState> LoadGame(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string firstLine = await reader.ReadLineAsync() ?? String.Empty;
                    string[] boardInfo = firstLine.Split(';');
                    int boardSize = Int32.Parse(boardInfo[0]);
                    int bombCount = Int32.Parse(boardInfo[1]);
                    int revealedCells = Int32.Parse(boardInfo[2]);

                    string firstPlayerIsNextString = await reader.ReadLineAsync() ?? String.Empty;
                    bool firstPlayerIsNext = Boolean.Parse(firstPlayerIsNextString);

                    MineFieldState loadedGameState = new MineFieldState(boardSize, bombCount, firstPlayerIsNext, revealedCells);

                    var boardSizeRange = Enumerable.Range(0, boardSize);
                    foreach (int verticalPosition in boardSizeRange)
                    {
                        foreach (int horizontalPosition in boardSizeRange)
                        {
                            string nextCellRepresentative = await reader.ReadLineAsync() ?? String.Empty;
                            string[] nextCellInfo = nextCellRepresentative.Split(';');
                            bool isRevealed = Boolean.Parse(nextCellInfo[2]);
                            bool isFlagged = Boolean.Parse(nextCellInfo[3]);
                            int neighbourBombCount = Int32.Parse(nextCellInfo[4]);
                            if (nextCellInfo[0].Equals(CellType.BOMB.ToString()))
                            {
                                loadedGameState[verticalPosition, horizontalPosition] = new Bomb(loadedGameState, verticalPosition, horizontalPosition, isRevealed, isFlagged, neighbourBombCount);
                            }
                            else if (nextCellInfo[0].Equals(CellType.SAFE.ToString()))
                            {
                                loadedGameState[verticalPosition, horizontalPosition] = new SafeCell(loadedGameState, verticalPosition, horizontalPosition, isRevealed, isFlagged, neighbourBombCount);
                            }
                        }
                    }

                    return loadedGameState;
                }
            }
            catch
            {
                throw new MineFieldDataException();
            }
        }
    }
}
