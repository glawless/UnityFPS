using System;
public interface GameMode
{
    void EndGame(); //calls any specified end game features unique to that game mode or
                    //passes it up to parent abstract class (Game)

    void CheckScore(); //ensures checking of limits set for score if flag enabled.
}
