# Dokumentation

> Lian Studer, 13.03.2022

## Inhaltsverzeichnis

[TOC]

## Einleitung (Management Summary)

**Autor**: Lian Studer

**Erstelldatum**: 13.04.2022

**Projektdauer**: 12.4.2022 bis 14.04.2022

#### Aufgabenstellung

Die Aufgabenstellung aus den Kursunterlagen lautet wortwörtlich wie folgt:

> In diesem ÜK geht es darum, eine Applikation zu programmieren, welche die Fahrplandaten des Schweizerischen öffentlichen Verkehrs benutzt. Mit der Applikation soll es möglich sein, Verkehrsverbindungen zwischen zwei Stationen zu suchen. Aus der Bewertung dieser Projektarbeit ergibt sich die ÜK-Note.

Der Auftrag ist also, eine Applikation für den Desktop zu entwickeln, welche die Swiss Public Transport API verwendet, um Informationen über den Öffentlichen Verkehr anzuzeigen.

#### Zweck dieses Dokuments

Dieses Dokument soll den Entwicklungsprozess der Projektarbeit im ÜK M318 dokumentieren. Es enthält die Planung (User Stories, Abnahmekriterien, Mockups, Aktivitätsdiagramm), allgemeine Informationen zum Projekt sowie die Definition der Testfälle mit Testprotokoll. Auch Informationen zum fertigen Produkt sind enthalten, wie beispielsweise bekannte Fehler/Bugs, oder fehlende Funktionalität.. Zusätzlich ist eine Installationsanleitung enthalten.

## Planung

### User Stories

In der folgenden Tabelle sind sämtliche User Stories aufgelistet, welche in diesem Projekt umgesetzt werden sollen. Die Tabelle ist nach Priorität der User Story sortiert. Jede User Story hat zusätzlich eine Liste von Abnahmekriterien, welche festlegen, wann eine User Story als implementiert und abgeschlossen gilt. Die letzte Spalte der Tabelle signalisiert, ob eine User Story abgeschlossen wurde, oder nicht. 

**Bedeutungen der Emojis:**

- Abgeschlossen: ✔️
- Nicht abgeschlossen: ❌

| ID    | User Story                                                   | Abnahmekriterien                                             | Priorität   | Status |
| ----- | ------------------------------------------------------------ | ------------------------------------------------------------ | ----------- | ------ |
| **1** | **Verbindungssuche**<br />Als Benutzer möchte ich mindestens die nächsten vier Verbindungen zwischen einer Start- und Endstation suchen können, um für mich eine passende Verbindung zu finden. | - Startstation in Textfeld eingeben <br />- Endstation in Textfeld eingeben<br />- Listet mindestens nächsten vier Verbindungen auf | 1           | ✔️      |
| **2** | **Abfahrtstafel**<br />Als Benutzer möchte ich eine Abfahrtstafel, um alle Verbindungen einer Startstation zu sehen. | - Startstation in Textfeld eingeben<br />- Anzeigen sämtlicher ausgehender Verbindungen von dort | 1           | ✔️      |
| **3** | **Stationssuche**<br />Als Benutzer möchte ich nach einer Station suchen können, um diese als Start- oder Endstation auswählen zu können. | - Anzeigen aller Stationen, die den Suchbegriff im Namen enthalten<br />- Passende Station kann als Start-/Endstation ausgewählt werden | 1           | ✔️      |
| **4** | **Stationssuche Autocomplete**<br />Als Benutzer möchte ich bei der Eingabe der Stationssuche automatisch Vorschläge erhalten, um eine Station, deren Namen ich nicht genau kenne, einfacher zu finden. | - Autovervollständigung der Eingabe bei Stationssuche<br />- Fuzzy Search | 2           | ✔️      |
| **5** | **Datum und Uhrzeit Filter**<br />Als Benutzer möchte ich ein Abfahrtsdatum und Uhrzeit eingeben können, um Verbindungen in der Zukunft einsehen zu können. | - Abfahrtsdatum auswählen<br />- Abfahrtszeit auswählen<br />- Entsprechende Verbindungen anzeigen | 2           | ✔️      |
| **6** | **Nächste Stationen**<br />Als Benutzer möchte ich die nächsten Stationen zu meinem Standort einsehen können, um zu wissen, wo ich am besten einsteigen soll. | - Standort erfassen<br />- Alle Stationen im nahen Umkreis anzeigen<br /> | 3           | ✔️      |
| **7** | **Stationen Karte**<br />Als Benutzer möchte ich eine Station auf einer Karte sehen können, um zu wissen, wo sich die Station befindet. | - Station auswählen<br />- Station auf interaktiver Karte einzeichnen | 3           | ❌      |
| **8** | **Verbindung teilen**<br />Als Benutzer möchte ich eine Verbindung per Email teilen können, um andere über eine Verbindung informieren zu können. | - Verbindung auswählen<br />- Textfeld für Empfänger-Email Adresse<br />- Textfeld für optionale Nachricht<br />- Uhrzeit, Start- und Endstation der Verbindung an Empfänger senden | 3           | ❌      |
| **9** | **Take Me Home**<br />Als Benutzer möchte ich einen "Take Me Home" Shortcut haben können, um mir die schnellste Verbindung von meinem aktuellen Standort zu meiner Heimadresse zu geben. | - Einstellung für Heimadresse<br />- Standort erfassen<br />- Nächste Startstation zum aktuellen Standort finden<br />- Nächste Endstation zu Heimadresse finden<br /> | Eigene Idee | ❌      |

### Aktivitätsdiagramm

Das untenstehende Bild ist ein Aktivitätsdiagramm, welches die Bedienung der Abfahrtstafel beschreibt.

<img src="C:\Work\M318\Projektarbeit\docs\assets\Aktivitaetsdiagram_Abfahrtstafel.svg" style="zoom:75%;

### Mockups

#### Abfahrtstafel

Das untenstehende Bild ist ein Mockup der Abfahrtstafel Funktion.

![Abfahrtstafel](assets/Mockup_UserStory2.png)

#### Verbindungssuche

Das untenstehende Bild ist ein Mockup der Verbindungssuche.

![Verbindungssuche](assets/Mockup_UserStory1.png)

## Umsetzung

### Implementation

Das Projekt wurde mit C# und WPF auf .NET 6 in Visual Studio 2022 entwickelt. Das Projekt verfolgt das MVVM Pattern und verwendet dazu die Library [Prism](https://prismlibrary.com). 

### Fehlende Funktionalität

Die folgende Tabelle enthält eine Auflistung an funktionalen und nicht-funktionalen Anforderungen, welche nicht umgesetzt werden konnten.

| Anforderung           | Anforderungsart         | Bemerkung                               |
| --------------------- | ----------------------- | --------------------------------------- |
| Stationen Karte       | Funktional (User Story) | Nicht umgesetzt                         |
| Verbindung teilen     | Funktional (User Story) | Nicht umgesetzt                         |
| Take Me Home          | Funktional (User Story) | Nicht umgesetzt                         |
| Datum und Zeit Filter | Funktional (User Story) | Es kann nur nach Datum gefiltert werden |

### Bekannte Fehler/Bugs

Die folgende Tabelle enthält eine Auflistung an bekannten Fehlern und Bugs bei der Ausführung, sowie Warnungen des Compilers.

#### Bugs

Es sind keine Bugs bekannt.

#### Compiler Warnungen

| Code   | Warnung                                                      | Bemerkung                                                    |
| ------ | ------------------------------------------------------------ | ------------------------------------------------------------ |
| CS8618 | Non-Nullable-Eigenschaft "City" muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie eine Deklaration von "Eigenschaft" als Nullable. | Das Model hat keinen Konstruktor. Visual Studio weiss nicht, dass dieses Feld nie NULL ist. |
| CS8618 | Non-Nullable-Eigenschaft "Country" muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie eine Deklaration von "Eigenschaft" als Nullable. | Das Model hat keinen Konstruktor. Visual Studio weiss nicht, dass dieses Feld nie NULL ist. |



## Tests

Dieser Abschnitt enthält das gesamte Testing der Applikation, inklusive Testprotokoll.

### Testfälle

#### Verbindungssuche

| ID    | Aktivität                                                    | Erwartetes Ergebnis                                          |
| ----- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **1** | Ich gebe "Wolhusen" in das Textfeld "From" ein und wähle Wolhusen aus den Vorschlägen aus. | Wolhusen wird als die Startstation ausgewählt. Weitere Stationen mit "Wolhusen" im Namen werden in den Vorschlägen aufgelistet. |
| **2** | Ich gebe "Entlebuch" in das Textfeld "To" ein und wähle Entlebuch aus den Vorschlägen aus. | Entlebuch wird als die Zielstation ausgewählt. Weitere Stationen mit "Entlebuch" im Namen werden in den Vorschlägen aufgelistet. |
| **3** | Ich klicke auf "Search connections".                         | Die zeitlich nächsten vier Verbindungen, nach der aktuellen Uhrzeit, die von Wolhusen nach Entlebuch fahren, werden angezeigt.. |

#### Abfahrtstafel

| ID    | Aktivität                                       | Erwartets Ergebnis                                           |
| ----- | ----------------------------------------------- | ------------------------------------------------------------ |
| **1** | Ich gebe "Wolhusen" in das Textfeld "From" ein. | Wolhusen wird als die Startstation ausgewählt. Weitere Stationen mit "Wolhusen" im Namen werden in den Vorschlägen aufgelistet. |
| **2** | Ich klicke den Button "Show departures".        | Es werden vier von Wolhusen ausgehende Verbindungen angezeigt. |

#### Stationssuche (mit Autocomplete)

| ID    | Aktivität                                              | Erwartets Ergebnis                                           |
| ----- | ------------------------------------------------------ | ------------------------------------------------------------ |
| **1** | Ich gebe "Wo" in die Textfelder "From" oder "To" ein.  | Es werden keine Vorschläge angezeigt.                        |
| **2** | Ich gebe "Wol" in die Textfelder "From" oder "To" ein. | Es erscheint ein Dropdown, in dem alle Stationen angezeigt, die "Wol" im Namen enthalten. |
| **3** | Ich wähle "Wolhusen, Spital" aus dem Dropdown aus.     | Die Station "Wolhusen, Spital" wird ausgewählt und als Text des entsprechenden Textfeldes ("From" oder "To") gesetzt. |

#### Datum und Uhrzeit Filter

| ID    | Aktivität                                                    | Erwartetes Ergebnis                                          |
| ----- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **1** | Ich gebe "Wolhusen" in das Textfeld "From" ein und wähle Wolhusen aus den Vorschlägen aus. | Wolhusen wird als die Startstation ausgewählt. Weitere Stationen mit "Wolhusen" im Namen werden in den Vorschlägen aufgelistet. |
| **2** | Ich gebe "Entlebuch" in das Textfeld "To" ein und wähle Entlebuch aus den Vorschlägen aus. | Entlebuch wird als die Zielstation ausgewählt. Weitere Stationen mit "Entlebuch" im Namen werden in den Vorschlägen aufgelistet. |
| **3** | Ich wähle den 3.8.2022 als das Abfahrtsdatum aus.            | Der 3.8.2022 wird als das Abfahrtsdatum festgelegt.          |
| **4** | Ich häkle die Checkbox "Apply filter" an.                    | Das festgelegte Abfahrtsdatum wird beim Suchen der Verbindungen angewendet. |
| **3** | Ich klicke auf "Search connections".                         | Die nächsten vier Verbindungen werden aufgelistet, welche am ausgewählten Abfahrtsdatum zur ausgewählten Abfahrtszeit von Wolhusen nach Entlebuch fahren. |

#### Nächste Stationen

| ID    | Aktivität                                  | Erwartetes Ergebnis                                          |
| ----- | ------------------------------------------ | ------------------------------------------------------------ |
| **1** | Ich gehe auf die Ansicht "Nearby stations" | Es werden zehn Stationen in der Nähe (IP Location) angezeigt. |



### Testprotokoll

Die folgende Tabelle protokolliert die Systemtests, durchgeführt durch eine Testperson, und deren Status.

**Bedeutungen der Emojis:**

- Bestanden: ✔️
- Nicht bestanden: ❌
- Nicht implementiert (kein Testfall): 🔘

| Test / Anforderung               | Getestet von   | Datum der Durchführung | Status |
| -------------------------------- | -------------- | ---------------------- | ------ |
| Verbindungssuche                 | Silas Reichlin | 14.04.2022             | ✔️      |
| Abfahrtstafel                    | Silas Reichlin | 14.04.2022             | ✔️      |
| Stationssuche (mit Autocomplete) | Silas Reichlin | 14.04.2022             | ✔️      |
| Datum und Uhrzeit Filter         | Silas Reichlin | 14.04.2022             | ❌      |
| Nächste Stationen                | Silas Reichlin | 14.04.2022             | ✔️      |
| Stationen Karte                  | Silas Reichlin | 14.04.2022             | 🔘      |
| Verbindung teilen                | Silas Reichlin | 14.04.2022             | 🔘      |
| Take Me Home                     | Silas Reichlin | 14.04.2022             | 🔘      |

## Installationseinleitung

Die Installation und Deinstallation läuft über den Installer in der Release Seite auf Github.
