INCLUDE globals.ink
EXTERNAL activateWallEvent()
EXTERNAL finalSceneTransition()

{ pokemon_name == "": -> main | -> already_chose}

VAR color = "\#323232"

=== main ===
Which pokemon do you <b><color=\#F8FF30>choose</color></b>?
    + [Charmander]
        ~ color = "\#FF0000"
        -> chosen("Charmander")
    + [Bulbasaur]
        ~ color = "\#339933"
        -> chosen("Bulbasaur")
    + [Squirtle]
        ~ color = "\#0099FF"
        -> chosen("Squirtle")

=== chosen(pokemon) ===
~ pokemon_name = pokemon
You chose <b><color={color}>{pokemon}</color></b>!
~ activateWallEvent()
~ finalSceneTransition()
-> END

=== already_chose ===
You already chose {pokemon_name}!
-> END
