
/// <summary>
///  classe pour tout les objet qui ont un mouvement.
/// </summary>
public class objetMobile : objetMere
{
	/// <summary>
	///  rajout de l'etat et de la direction.
	/// </summary>
	protected string _direction;
	protected string _etat;


	public objetMobile() :base(){

		this.setDirection("");
		this.setEtat ("vivant");
	}
	public objetMobile(float x, float y, string direction) :base(x, y){
		this.setDirection (direction);
		this.setEtat ("vivant");
	}

	/// <summary>
	///  accesseur en lecture
	/// </summary>
	public string getDirection(){
		return _direction;
	}

	public string getEtat(){
		return _etat;
	}

	/// <summary>
	///  accesseur en ecriture.
	/// </summary>
	public void setDirection(string direction){
		_direction = direction;
	}



	public void setEtat(string etat){
		_etat=etat;
	}

}	

