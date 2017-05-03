# GiveOrTake

An application to list items lent or borrowed to/from friends. As students, we share notes, data in thumb drives, etc., and sometimes forget to whom we gave it to. 

This application aims to help remember where our belongings are. 

The user can add an entry that she/he has given or taken something (a transaction), and an expected return date. This expected return date will trigger a notification reminding the user to return the item. 

Database:

- The Database will consist of a list of users.
- Each user's unique ID and name is required.
- A transaction is the event where an item (a book, thumb drive or money) is lent or borrowed. A transaction will have an ID, an item name which was exchanged, a short description, an occurence date, an expected return date, name of the recipient and a transaction type (which is a 'Give' or a 'Take')
- A user can have multiple transactions.
