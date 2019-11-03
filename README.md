# TP1Distribution
Git pour le projet "SiteWeb de Film" pour le TP1 de Distribution.

1.Prendre le fichier 7Zip/Zip et le décompresser dans un dossier de votre choix.

2.Pour accéder au projet dans visual studio, allez dans movieGEN/movieGEN.sln.

3.Clé API:
	a)OMDB movie: pour obtenir votre clé API, allez sur http://www.omdbapi.com/apikey.aspx, 
	choisissez l'option FREE! (1,000 daily limit) et mettez votre mail par lequel 
	vous voulez recevoir la clé.
	
	b)Pour avoir une clé API pour "Youtube Data API V3", il faut se faire un compte google developers sur 
	https://console.developers.google.com/, ensuite se faire un nouveau projet pour y mettre les API qu'on veut,
	aller dans Bibliothèque d'API, choisir "YouTube Data API v3", l'activer, se "Créer des Identifiants",
	Remplir le formulaire nécéssaire pour avoir cette API: Dans l'option d'ou on appèle l'API mettez
	Web Browser javascript et choisissez "Donnée publique" avant de complèter le formulaire. Après la clé 
	apparait, gardez-là en note.

	c)Ebay API: pour obtenir cette clé, il faut être un membre du cercle de développeur "https://developer.ebay.com/signin"
	il faudra vous y inscrire. À l'une des étapes, il faut y mettre le nom de l'application, ne mettez pas n'importe
	quoi. Demander à créer une clé de production(pas sandbox) une fois sur cette page (https://developer.ebay.com/my/keys).
	Vous avez votre clé!

	d)Google traduction(Cloud Translation API):On peut obtenir cette clé sur https://console.developers.google.com/ dans la bibliothèque.
	Une fois l'API activé allez dans gérer, clicker sur votre compte de service(si vous n'en avez pas faite en un), aller dans clé et "Créer une clé",
	prenez le format JSON et créer-là. Gardez le fichier téléchargé.
	Attention, il faut payer pour avoir cette API et la clé(6 mois gratuit), il faut activé la facturation et prendre un compte gratuit pour
	ceux qui ne veulent pas payé.

4. Ouvrez le projet.
	a)Pour OMDB movie, allez dans le HomeController, à la ligne 39, 165,214-223, la clé dans les liens devra être changé par votre clé(?apikey="APIKEY"&t...).
	b)Pour youtube, le package nuget est déjà installer, tout ce que vous avez à faire c'est de changer la clé à la ligne 186 du HomeController par votre clé.
	c)Pour Ebay, dans HomeController, à la ligne 102 dans "&SECURITY-APPNAME=" mettez le App ID (Client ID) de la clé de production du site de developpeur d'EBAY(https://developer.ebay.com/my/keys) dans lequel vous avez généré les clés.
	d)Pour Google traduction, prenez votre fichier .json précédemment télécharger avec votre clé à l'intérieure et mettez le dans le dossier movieGEN au dessus de la solution avec les autres fichier .json
	  Ensuite,  allez dans startup.cs dans le projet à la ligne 46 et remplacer le nom de fichier .json qui s'y trouve par le vôtre.

