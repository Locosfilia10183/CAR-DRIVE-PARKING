# Erste spielbare Neustadt-Version

## Ziel
Dieses Skript verbindet den vorhandenen District-01-Greybox-Aufbau mit einem fahrbaren Spielerauto. Es ist ein erster Offline-Fahrtest, noch kein fertiger Spielmodus.

## Einrichtung in Unity
1. Öffne die Szene `Neustadt_District_01`.
2. Wähle das GameObject `NeustadtMapBootstrap`, auf dem bereits `NeustadtMapBuilder` sitzt.
3. Füge `NeustadtPlayableBootstrap` als weitere Komponente auf demselben Objekt hinzu.
4. Optional können `NeustadtStreetDetails` und `NeustadtBuildingDetails` ebenfalls dort bleiben.
5. Speichere die Szene und drücke Play.

Der spätere Ausführungszeitpunkt sorgt dafür, dass der Map Builder zuerst seine Karte und Übersichtskamera erzeugt. Der Playable Bootstrap schaltet die Übersichtskamera ab und erzeugt das Spielerauto.

## Steuerung
- **W / Pfeil hoch:** Gas
- **S / Pfeil runter:** bremsen und anschließend rückwärts fahren
- **A/D oder Pfeile links/rechts:** lenken
- **Leertaste:** Handbremse
- **C:** Außen-/Innenkamera wechseln
- **Touch:** Bildschirmtasten für Lenken, Gas, Bremse, Handbremse und Kamera

Das Auto startet auf dem Parkplatz im Südosten des Bezirks.


## Fahrtest-Verbesserungen
- Asphalt der Hauptallee, Ringstraße und des Parkplatzes besitzt jetzt Kollisionsflächen, damit die Räder auf der Straßenoberfläche statt auf dem tiefer liegenden Terrain rollen.
- Die eingestellte Höchstgeschwindigkeit wird nun vorwärts und rückwärts begrenzt.

## Bekannte Grenzen
- Das ist ein einfacher Fahr-Prototyp mit WheelCollidern, kein realistisches Fahrzeugmodell.
- KI-Verkehr, Fußgänger, Missionen, Speichern, Garage und Multiplayer sind noch nicht enthalten.
- Die Karte nutzt bisher Greybox-Kollisionsflächen. Fahrverhalten und Spawn-Höhe müssen im Unity Editor geprüft und ggf. angepasst werden.
- Der Touch-UI-Aufbau verwendet Unitys Legacy UI und Standalone Input Module. Bei einem Projekt, das ausschließlich das neue Input System aktiviert hat, muss das UI-Input-Modul entsprechend umgestellt werden.

**Teststatus:** Die Skripte sind in GitHub gespeichert, aber nicht in Unity kompiliert oder ausgeführt. Bitte erst im Editor testen, bevor weitere Fahrphysik darauf aufbaut.
