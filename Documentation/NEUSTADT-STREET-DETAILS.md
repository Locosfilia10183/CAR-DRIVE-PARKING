# Neustadt – Straßendetails (District 01)

## Inhalt
Dieses optionale Zusatzskript ergänzt den ersten Neustadt-Greybox-Bezirk um:

- vier statische Ampel-Aufbauten an der Kreuzung
- Tempo-50-Schild-Blockouts
- Parken- und Stoppschild-Blockouts
- blaue Straßennamenschilder: Hafenstrasse und Neustädter Ring
- eine einfache Bushaltestelle mit Wartehäuschen

Die Ampeln sind aktuell nur Dekoration. Sie wechseln nicht und steuern keinen Verkehr. Die Schilder sind geometrische Platzhalter, keine finalen, maßstabsgetreuen deutschen Verkehrszeichen.

## Einrichtung in Unity
1. Öffne die Szene `Neustadt_District_01`.
2. Wähle das GameObject `NeustadtMapBootstrap`.
3. Füge zusätzlich zum `NeustadtMapBuilder` die Komponente `NeustadtStreetDetails` hinzu.
4. Lass `Build On Start`, `Add Traffic Lights`, `Add Road Signs`, `Add Street Name Signs` und `Add Bus Stop` aktiviert.
5. Starte Play Mode.

Das Skript erzeugt seine Objekte zur Laufzeit. Es ersetzt weder den Map Builder noch erzeugt es eine gespeicherte Unity-Szene.

## Noch offen
- Ampelphasen und Kreuzungslogik
- echte Verkehrszeichen-Meshes und lesbare Tempoangaben
- Buslinien, Fahrpläne und ein funktionierender ÖPNV
- Kollisions- und Maßstabsprüfung in Unity
- finale Materialien, Texturen und Beleuchtung

**Teststatus:** Noch nicht in Unity kompiliert oder im Editor geprüft. Das ist ein zusätzlicher Greybox-Baustein im GitHub-Repository.
