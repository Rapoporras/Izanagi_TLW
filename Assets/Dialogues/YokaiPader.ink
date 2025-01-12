INCLUDE globals.ink
EXTERNAL activateWallEvent()

{ getTea == false: -> main | -> goOut}


=== main ===
~ meetYokai = true
¿Qué haces aquí? <i>¡No me moveré!</i> Tengo un hambre terrible
Sin el <b>Ramen especial</b>, no puedo dar ni un paso.
En estas profundidades, vive un kappa que escapó de la maldición de los onis. 
Él debe tener un poco de <b>Ramen especial</b>
Tráemelo, y tal vez te deje pasar.
 ->END




=== goOut ===
¡Oh, eso es <b>Ramen especial</b>! !Damelo!
¡Agg! ¡Qué asco! ¿De verdad esperé todo este tiempo por esto?
Me dijeron que era un manjar... pero vaya decepción. 
En fin, me largo de aquí.
~ yokaiOut = true
~ activateWallEvent()
-> END

