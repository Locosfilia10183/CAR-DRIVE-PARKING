# Neustadt – Gebäudefassaden (District 01)

## Ergänzungen
Das optionale Skript `NeustadtBuildingDetails` ergänzt die bestehenden Gebäudeblockouts um:

- Dachkanten und einfache architektonische Abschlüsse
- Balkone an den vier Wohngebäuden
- Café-Schaufenster, rotes Vordach und Schild
- Autohaus-Glasfront und Beschriftung
- drei Werkstatt-Rolltore mit Lamellen
- Glaseingang und Vordach am City Office

Alle Elemente sind dekorative Greybox-Geometrie ohne zusätzliche Kollisionskörper. Es werden keine bestehenden Gebäude verändert.

## Einrichtung
1. Szene `Neustadt_District_01` öffnen.
2. Das Objekt `NeustadtMapBootstrap` auswählen.
3. `NeustadtBuildingDetails` als zusätzliche Komponente hinzufügen.
4. `Build On Start` aktiviert lassen und Play drücken.

Das Skript ist zusätzlich zum `NeustadtMapBuilder` und `NeustadtStreetDetails` gedacht.

## Noch offen
- echte Tür- und Fensterrahmen, Materialien und Texturen
- korrekte Balkongeometrie und Maßstabsprüfung
- Innenräume und begehbare Gebäude
- individuelle Fassaden für alle Gebäude
- Unity-Kompilierung und Sichtprüfung

**Teststatus:** Noch nicht in Unity kompiliert oder im Editor geprüft.
