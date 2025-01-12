INCLUDE globals.ink

{ meetYokai == false: -> main | -> talkYokai}


=== main ===
Llevo años vagando por las profundidades de esta cueva... y es la primera vez que mis ojos ven a un mortal. 
¿Quieres algo de comer?
 ->END

=== talkYokai ===

{ getTea == false: -> giveTea | -> haveTea}

-> END


=== giveTea ===
~ getTea = true
¿Quieres ayudar a ese llorón bloqueador de caminos? Hmm... tengo algo de <b>Ramen especial</b>, pero ya no lo necesito.
Toma, te lo doy. 
Considera esto un regalo por ser mi primer cliente humano.
Solo espero que ese obstáculo andante no vuelva a molestar pronto.
-> END

=== haveTea ===
Ya no tengo nada más, aunque puedes volver mas tarde.
-> END