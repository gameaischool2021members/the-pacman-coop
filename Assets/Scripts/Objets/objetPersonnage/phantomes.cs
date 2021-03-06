using System;
using UnityEngine;
/// <summary>
///  classe permettant de manipuler l'Ia des fantomes.
/// </summary>
public class phantomes :objetMobile
{
	/// <summary>
	///  variables de distances pour les trois possibles positions du fantome.
	/// </summary>
	protected double _distance1;
	protected double _distance2;
	protected double _distance3;
	protected int _orientation;

	/// <summary>
	///  constructeurs.
	/// </summary>
	public phantomes() : base(){
	}


	public phantomes(float x, float y, string direction) : base(x , y ,direction){
	}

	/// <summary>
	///  accesseur en éciture.
	/// </summary>
	public void setDistance1(double d){
		_distance1 = d;
	}
	public void setDistance2(double d){
		_distance2 = d;
	}
	public void setDistance3(double d){
		_distance3 = d;
	}
	public void setOrientation(int d){
		_orientation = d;
	}

	/// <summary>
	///  accesseur en lecture.
	/// </summary>
	public double getDistance1(){
		return _distance1;
	}
	public double getDistance2(){
		return _distance2;
	}
	public double getDistance3(){
		return _distance3;
	}
	public double getOrientation(){
		return _orientation;
	}


	/// <summary>
	///  renvoi une valeur correspondant à sois la distance entre pacman et la position données a la fonction sois la plus grande valeur possible si a la position donné c'est un mur.
	/// </summary>
	public double traqueP( double xp,double yp,float xf,float yf,TileMatrix M){
		RaycastHit2D rayhit;
		rayhit = Physics2D.Raycast (new Vector2 (xf, yf), new Vector2 (xf, yf), 0.1f);
		if (rayhit && rayhit.collider.tag == "mur") {
			
			return M.max();	

		} else {

			return Math.Sqrt (((xp - xf) * (xp - xf)) + ((yp - yf) * (yp - yf)));
		}
	}
	/// <summary>
	///  renvoi la plus grande valeur parmis trois (1,2,3) ou 4 si elles sont toute égales.
	/// </summary>
	public int Test(double distance1,double distance2, double distance3, TileMatrix M){
		if (distance1 <= distance2 && distance1 <= distance3 && distance1 != M.max ()) {
			return 1;
		} else {
			if (distance2 <= distance1 && distance2 <= distance3 && distance2 != M.max ()) {
				return 2;
			} else {
				if (distance3 <= distance1 && distance3 <= distance2 && distance3 != M.max ()) {
					return 3;
				} else {
					return 4;
				}
			}
		}
	}
	/// <summary>
	///  cette fonction fais le lien entre toute les fonction si dessus elle permet de trouver le chemin le plus court entre le fantome (variable fantome) et la position voullu (variable pacman qui est une position).
	/// </summary>
	public void rechercheR( Transform fantome,Vector2 pacman, TileMatrix M){

	/// <summary>
	///  si il n'y a pas de direction au fantome (qu'il est immobile) alors calcul la distance entre ses prochaine position possible et pacman
	/// </summary>
		if (this.getDirection() == "") {
		/// <summary>
		///  en haut
		/// </summary>
			this.setDistance1( this.traqueP ((pacman.x), (pacman.y) , (fantome.transform.position.x), (fantome.transform.position.y) + 0.24f, M));
		/// <summary>
		///  a droite
		/// </summary>
			this.setDistance2( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) + 0.24f, (fantome.transform.position.y), M));
		/// <summary>
		///  en bas
		/// </summary>
			this.setDistance3 (this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x), (fantome.transform.position.y) - 0.24f, M));

		/// <summary>
		///  détermine la distance la plus courte
		/// </summary>
			this.setOrientation( this.Test (this.getDistance1(), this.getDistance2(), this.getDistance3(),M));
		/// <summary>
		///  Donne la direction trouvé au fantome.
		/// </summary>
			if (this.getOrientation() == 1) {
				this.setDirection ("haut");
			}
			if (this.getOrientation() == 2) {
				this.setDirection ("droite");
			}
			if (this.getOrientation() == 3) {
				this.setDirection ("bas");
			}
			if (this.getOrientation() == 4)  {
				this.setDirection ( "gauche");
			}
		}else{
		/// <summary>
		///  si la directionest = a droite au fantome (qu'il est immobile) alors calcul la distance entre ses prochaine position possible et pacman
		/// </summary>
			if (this.getDirection() == "droite") {
			/// <summary>
			///  en haut
			/// </summary>
				this.setDistance1( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x), (fantome.transform.position.y) + 0.24f, M));
			/// <summary>
			///  en droite
			/// </summary>
				this.setDistance2( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) + 0.24f, (fantome.transform.position.y), M));
			/// <summary>
			///  en bas
			/// </summary>
				this.setDistance3 (this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x), (fantome.transform.position.y) - 0.24f, M));

			/// <summary>
			///  détermine la distance la plus courte
			/// </summary>
				this.setOrientation( this.Test (this.getDistance1(), this.getDistance2(), this.getDistance3(),M));

			/// <summary>
			///  Donne la direction trouvé au fantome.
			/// </summary>
				if (this.getOrientation() == 1) {
					this.setDirection ("haut");
				}
				if (this.getOrientation() == 2) {
					this.setDirection ("droite");
				}
				if (this.getOrientation() == 3) {
					this.setDirection ("bas");
				}
				if (this.getOrientation() == 4)  {
					this.setDirection ( "gauche");
				}
			}else{
			
			/// <summary>
			///  si la directionest = a droite au fantome (qu'il est immobile) alors calcul la distance entre ses prochaine position possible et pacman
			/// </summary>
				if (this.getDirection() == "bas") {
				/// <summary>
				///  à droite
				/// </summary>
					this.setDistance1( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) + 0.24f, (fantome.transform.position.y), M));
				/// <summary>
				///  en bas
				/// </summary>
					this.setDistance2( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x), (fantome.transform.position.y) - 0.24f, M));
				/// <summary>
				///  a gauche
				/// </summary>
					this.setDistance3 (this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) - 0.24f, (fantome.transform.position.y), M));

				/// <summary>
				///  détermine la distance la plus courte
				/// </summary>
					this.setOrientation( this.Test (this.getDistance1(), this.getDistance2(), this.getDistance3(),M));

				/// <summary>
				///  Donne la direction trouvé au fantome.
				/// </summary>
					if (this.getOrientation() == 1) {
						this.setDirection ("droite");
					}
					if (this.getOrientation() == 2) {
						this.setDirection ("bas");
					}
					if (this.getOrientation() == 3) {
						this.setDirection ("gauche");
					}
					if (this.getOrientation() == 4)  {
						this.setDirection ( "haut");
					}
				}else{

				/// <summary>
				///  si la directionest = a droite au fantome (qu'il est immobile) alors calcul la distance entre ses prochaine position possible et pacman
				/// </summary>
					if (this.getDirection() == "gauche") {
					
					/// <summary>
					///  en bas
					/// </summary>
						this.setDistance1( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x), (fantome.transform.position.y) - 0.24f, M));

					/// <summary>
					///  à gauche
					/// </summary>
						this.setDistance2( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) - 0.24f, (fantome.transform.position.y), M));

					/// <summary>
					///  en haut
					/// </summary>
						this.setDistance3( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x), (fantome.transform.position.y) + 0.24f, M));
						
					/// <summary>
					///  détermine la distance la plus courte.
					/// </summary>
						this.setOrientation( this.Test (this.getDistance1(), this.getDistance2(), this.getDistance3(),M));

					/// <summary>
					///  Donne la direction trouvé au fantome.
					/// </summary>
						if (this.getOrientation() == 1) {
							this.setDirection ("bas");
						}
						if (this.getOrientation() == 2) {
							this.setDirection ("gauche");
						}
						if (this.getOrientation() == 3) {
							this.setDirection ("haut");
						}
						if (this.getOrientation() == 4)  {
							this.setDirection ( "droite");
						}
					}else{

					/// <summary>
					///  si la directionest = a droite au fantome (qu'il est immobile) alors calcul la distance entre ses prochaine position possible et pacman
					/// </summary>
						if (this.getDirection()=="haut") {

						/// <summary>
						///  à gauche
						/// </summary>
							this.setDistance1( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) - 0.24f, (fantome.transform.position.y) , M));

						/// <summary>
						///  en haut
						/// </summary>
							this.setDistance2( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) , (fantome.transform.position.y) + 0.24f, M));

						/// <summary>
						///  à droite
						/// </summary>
							this.setDistance3( this.traqueP ((pacman.x), (pacman.y), (fantome.transform.position.x) + 0.24f, (fantome.transform.position.y), M));

						/// <summary>
						///  détermine la distance la plus courte
						/// </summary>
							this.setOrientation( this.Test (this.getDistance1(), this.getDistance2(), this.getDistance3(),M));

						/// <summary>
						///  Donne la direction trouvé au fantome.
						/// </summary>
							if (this.getOrientation() == 1) {
								this.setDirection ("gauche");
							}
							if (this.getOrientation() == 2) {
								this.setDirection ("haut");
							}
							if (this.getOrientation() == 3) {
								this.setDirection ("droite");
							}
							if (this.getOrientation()==4)
							{
								this.setDirection ("bas");
							}
						}
					}
				}
			}
		}
	}
}



