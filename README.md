# TP1Distribution
Git pour le projet "SiteWeb de Film" pour le TP1 de Distribution.

1.Prendre le fichier 7Zip/Zip et le d�compresser dans un dossier de votre choix.

2.Pour acc�der au projet dans visual studio, allez dans movieGEN/movieGEN.sln.

3.Cl� API:
	a)OMDB movie: pour obtenir votre cl� API, allez sur http://www.omdbapi.com/apikey.aspx, 
	choisissez l'option FREE! (1,000 daily limit) et mettez votre mail par lequel 
	vous voulez recevoir la cl�.
	
	b)Pour avoir une cl� API pour "Youtube Data API V3", il faut se faire un compte google developers sur 
	https://console.developers.google.com/, ensuite se faire un nouveau projet pour y mettre les API qu'on veut,
	aller dans Biblioth�que d'API, choisir "YouTube Data API v3", l'activer, se "Cr�er des Identifiants",
	Remplir le formulaire n�c�ssaire pour avoir cette API: Dans l'option d'ou on app�le l'API mettez
	Web Browser javascript et choisissez "Donn�e publique" avant de compl�ter le formulaire. Apr�s la cl� 
	apparait, gardez-l� en note.

	c)Ebay API: pour obtenir cette cl�, il faut �tre un membre du cercle de d�veloppeur "https://developer.ebay.com/signin"
	il faudra vous y inscrire. � l'une des �tapes, il faut y mettre le nom de l'application, ne mettez pas n'importe
	quoi. Demander � cr�er une cl� de production(pas sandbox) une fois sur cette page (https://developer.ebay.com/my/keys).
	Vous avez votre cl�!

	d)Google traduction(Cloud Translation API):On peut obtenir cette cl� sur https://console.developers.google.com/ dans la biblioth�que.
	Une fois l'API activ� allez dans g�rer, clicker sur votre compte de service(si vous n'en avez pas faite en un), aller dans cl� et "Cr�er une cl�",
	prenez le format JSON et cr�er-l�. Gardez le fichier t�l�charg�.
	Attention, il faut payer pour avoir cette API et la cl�(6 mois gratuit), il faut activ� la facturation et prendre un compte gratuit pour
	ceux qui ne veulent pas pay�.

4. Ouvrez le projet.
	a)Pour OMDB movie, allez dans le HomeController, � la ligne 39, 165,214-223, la cl� dans les liens devra �tre chang� par votre cl�(?apikey="APIKEY"&t...).
	b)Pour youtube, le package nuget est d�j� installer, tout ce que vous avez � faire c'est de changer la cl� � la ligne 186 du HomeController par votre cl�.
	c)Pour Ebay, dans HomeController, � la ligne 102 dans "&SECURITY-APPNAME=" mettez le App ID (Client ID) de la cl� de production du site de developpeur d'EBAY(https://developer.ebay.com/my/keys) dans lequel vous avez g�n�r� les cl�s.
	d)Pour Google traduction, prenez votre fichier .json pr�c�demment t�l�charger avec votre cl� � l'int�rieure et mettez le dans le dossier movieGEN au dessus de la solution avec les autres fichier .json
	  Ensuite,  allez dans startup.cs dans le projet � la ligne 46 et remplacer le nom de fichier .json qui s'y trouve par le v�tre.

