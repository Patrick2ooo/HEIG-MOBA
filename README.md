# Ressurection

## Comment contribuer au projet

Afin de contribuer au code, il faut tout d'abord créer une branche depuis dev. Vous pourrez ensuite implémenter les changements souhaités sur celle-ci, la push, puis la merge dans dev de nouveau.

Lors des pushs, les tests sont lancés afin de vérifier que tout fonctionne. Lors d'un merge, les builds PC et Android sont créés.

## Structure du projet

Toutes les ressources en lien avec le projet se trouvent dans le dossier MOBA. Les scripts se trouvent dans MOBA/Assets/Scripts et les scènes dans MOBA/Assets/Scripts.

## Comment setup votre machine pour contribuer au jeu

Vous devez installer la version 2022.3.37f1 de Unity afin de pouvoir ouvrir le projet. Il est recommandé d'utiliser un IDE pour modifier les scripts C# sur votre machine, par défaut Visual Studio est fortement recommandé, mais nombre d'entre nous avons utilisé Rider ou Visual Studio Code à la place. Notez cependant qu'il est fortement recommandé d'installer un plugin Unity dans tous les cas afin que votre IDE connaisse les différentes méthodes des packages Unity.

Pour configurer votre IDE sur unity : Edit > preferences > External tools > puis il faut définir votre IDE dans External Script Editor et regenerer les fichiers du projet.

## Comment contribuer
Si une contribution doit être faite, il faut créer une branche pour chaque modification que nous souhaitons apporter. Ces modifications pourront être merge sur dev lorsequ'elle seront fonctionnelles.

## Licences / AssetStore
Dans ce repo des assets de l'unity store ont été utilisés et importés dans le projet à des fins de test. Avoir accès à ce repo ne donne pas droit à l'utilisation de ces assets. Ils sont trouvables dans le dossier AssetStore. Il s'agit d'icônes.