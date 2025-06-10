# Exercice – Implémentation de Détection d’Abus de Retours

## Contexte 
Notre application permet de gérer des retours de produit en entrepôt (création et consultation)

**NB:** Pour vous familiariser avec l'application, vous pouvez la lancer et faire des requêtes. 

## Nouvelle fonctionnalité 
Notre équipe doit mettre en place une fonctionnalité permettant d’identifier les clients abusant des retours.
Un client est considéré comme abusif s’il a **réellement** retourné plus de $N$ retours sur une période donnée (on se base sur la date de création de retour). 

*Remarque: Un retour est dit *réellement* retourné si son statut est `Receipt` ou `Qualified`.*

### À faire
Créer deux endpoints:

1. **Premier endpoint:** Demande de création d'un rapport via 
`POST /abuse-detection-reports`

Il prend en body: 
```json
{
  "threshold": 3, 
  "startDate": "2025-06-01",
  "endDate": "2025-06-05"
}
```

Cette demande de rapport sera enregistrée et elle aura un identifiant unique à retourner dans le header Location. 

2. **Deuxième endpoint:** Le résultat sera consultable via `GET /abuse-detection-reports/{id}`

Quand le calcul est en cours, le résultat affiché doit être:

```json
{
  "detectionId": "abcd-1234",
  "status": "InProgress",
  "threshold": 3,
  "startDate": "2025-06-01",
  "endDate": "2025-06-05",
  "abusiveCustomers": null
}
```

Quand le calcul est terminé, le résultat doit être:
```json
{
  "detectionId": "abcd-1234",
  "status": "Completed",
  "threshold": 3,
  "startDate": "2025-06-01",
  "endDate": "2025-06-05",
  "abusiveClientIds": ["C1", "C2"]
}
```

**Exemple**

Pour les données suivantes:
```
Client C1:
- Return R1: Status=Receipt, CreationDate=2025-06-01
- Return R2: Status=Qualified, CreationDate=2025-06-05

Client C2:
- Return R3: Status=Created, CreationDate=2025-06-01
- Return R4: Status=Created, CreationDate=2025-06-02
- Return R5: Status=Created, CreationDate=2025-06-03
- Return R6: Status=Created, CreationDate=2025-06-04
- Return R7: Status=Receipt, CreationDate=2025-06-05

Client C3:
- Return R8: Status=Receipt, CreationDate=2025-06-01
- Return R9: Status=Receipt, CreationDate=2025-06-03
- Return R10: Status=Qualified, CreationDate=2025-06-15
- Return R11: Status=Qualified, CreationDate=2025-06-17

Client C4:
- Return R12: Status=Receipt, CreationDate=2025-06-01
- Return R13: Status=Receipt, CreationDate=2025-06-03
- Return R14: Status=Qualified, CreationDate=2025-06-05
- Return R15: Status=Qualified, CreationDate=2025-06-07
```

Et la requête:
```json
{
  "threshold": 3,
  "startDate": "2025-06-01",
  "endDate": "2025-06-07"
}
```

À la fin du traitement, la réponse doit être:
```json
{
  "detectionId": "59f2e8d7-1234-4ab7-9876-c0d1e2f3a4b5",
  "status": "Completed",
  "threshold": 3,
  "startDate": "2025-06-01",
  "endDate": "2025-06-07",
  "abusiveClientIds": ["C4"]
}
```

**Explication**

1. **Client C1** possède seulement 2 retours valides (`Receipt` et `Qualified`), ce qui est inférieur au seuil de 3, donc il n'est pas considéré comme abusif.

2. **Client C2** a un total de 5 retours, mais seulement 1 est valide (`Receipt`), car les 4 autres sont dans l'état `Created`. Puisque nous ne comptons que les retours en état `Receipt` et `Qualified`, il n'est pas considéré comme abusif.

3. **Client C3** a 4 retours valides (2 `Receipt`, 2 `Qualified`) au total, ce qui dépasse le seuil de 3. Cependant, ils sont répartis en dehors de la plage donnée (retours aux jours 01, 03, 15 et 17), donc il n'est pas considéré comme abusif selon notre critère de plage du temps.

4. **Client C4** a 4 retours valides (2 `Receipt`, 2 `Qualified`) dans la plage donnée (retours aux jours 01, 03, 05 et 07), dépassant le seuil de 3. En conséquence, il est considéré comme abusif.

