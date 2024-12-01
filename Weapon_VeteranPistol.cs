//audio
datablock AudioProfile(VeteranPistolFireSound)
{
	filename    = "./Sounds/VeteranPistol_Fire.wav";
	description = AudioDefault3d;
	preload = true;
};

// datablock AudioProfile(PistolClickSound)
// {
//	 filename    = "./Pistol_click.wav";
//	 description = AudioClosest3d;
//	 preload = true;
// };

datablock ProjectileData(VeteranPistolProjectile : TTOBulletProjectile)
{
	directDamage        = 17;
	directDamageType    = $DamageType::VeteranPistol;
	radiusDamageType    = $DamageType::VeteranPistol;

	impactImpulse	     = 100;
	verticalImpulse	  = 50;

	muzzleVelocity      = 120;

   gravityMod = 0.4;
};

//////////
// item //
//////////
datablock ItemData(VeteranPistolItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Models/VeteranPistol.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Veteran Pistol";
	iconName = "./Icons/VeteranPistol";
	doColorShift = true;
	colorShiftColor = "0.54 0.52 0.47 1";

	 // Dynamic properties defined by the scripts
	image = ServicePistolImage;
	canDrop = true;

	TTO_ammoType = "45ACP";
	TTO_reloads = true;
	TTO_maxAmmo = 7;
};

AddDamageType("VeteranPistol",   '<bitmap:add-ons/Weapon_Package_TTOverhaul_Guns/Icons/CI_ServicePistol> %1 has seem ot all',    '%2  <bitmap:add-ons/Weapon_Package_TTOverhaul_Guns/Icons/CI_ServicePistol> %1 Saw a Veteran in battle',0.75,1);

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(VeteranPistolImage)
{
	// Basic Item properties
	shapeFile = "./Models/VeteranPistol.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0; //"0.7 1.2 -0.5";
	rotation = eulerToMatrix( "0 0 0" );

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.  
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = VeteranPistolItem;
	ammo = " ";
	projectile = VeteranPistolProjectile;
	projectileType = Projectile;

	casing = GunShellDebris;
	shellExitDir        = "1.0 0.1 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 10.0;	
	shellVelocity       = 5.0;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	doColorShift = true;
	colorShiftColor = VeteranPistolItem.colorShiftColor;

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.15;
	stateTransitionOnTimeout[0]      = "LoadCheckA";
	stateSound[0]                    = weaponswitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnNotLoaded[1]    = "ManualReload";
	stateTransitionOnTriggerDown[1]  = "FireCheckA";

	// Switching from TTAmmo 1/2 to other modes and back may leave the gun in "Ready" when it is empty
	// Fire check prevents the gun from firing in this case
	// For some reason we can get away with no timeout here
	stateName[2]                     = "FireCheckA";
	stateScript[2]                   = "TTO_onFireCheck";
	stateTransitionOnTimeout[2]      = "FireCheckB";

	stateName[3]                     = "FireCheckB";
	stateTransitionOnLoaded[3]       = "Fire";
	stateTransitionOnNotLoaded[3]    = "EmptyFire";

	stateName[4]                     = "Fire";
	stateTransitionOnTimeout[4]      = "Smoke";
	stateTimeoutValue[4]             = 0.05;
	stateFire[4]                     = true;
	stateAllowImageChange[4]         = false;
	stateEjectShell[4]               = true;
	stateScript[4]                   = "onFire";
	stateSequence[4]                 = "Fire";
	stateEmitter[4]                  = TTOMuzzleFlashEmitter;
	stateEmitterTime[4]              = 0.05;
	stateEmitterNode[4]              = "muzzleNode";
	//stateSound[4]                    = PistolfireSound;

	stateName[5]                     = "Smoke";
	stateEmitter[5]                  = gunSmokeEmitter;
	stateEmitterTime[5]              = 0.2;
	stateEmitterNode[5]              = "muzzleNode";
	stateTransitionOnTriggerUp[5]    = "Wait";

	stateName[6]                     = "Wait";
	stateTimeoutValue[6]             = 0.034;
	stateScript[6]                   = "onBounce";
	stateTransitionOnTimeout[6]      = "LoadCheckA";
	//stateSound[6]                    = pistolClickSound;

	stateName[7]                     = "LoadCheckA";
	stateTimeoutValue[7]             = 0.01;
	stateScript[7]                   = "TTO_onLoadCheck";
	stateTransitionOnTimeout[7]      = "LoadCheckB";

	stateName[8]                     = "LoadCheckB";
	stateTransitionOnLoaded[8]       = "Ready";
	stateTransitionOnNotLoaded[8]    = "Empty";
	
	//Torque switches states instantly if there is an ammo/noammo state, regardless of stateWaitForTimeout
	
	// Loaded - gun has ammo, doesn't need reload
	// NotLoaded - empty gun, needs reload
	// Ammo - reserve ammo available, can reload
	// NoAmmo - no reserve ammo left, can't reload
	stateName[9]                     = "ReloadWait";
	stateTimeoutValue[9]             = 0.3;
	stateScript[9]                   = "onReloadWait";
	stateTransitionOnTimeout[9]      = "ReloadStart";

	// When splitting up the Timeout of a state only 32 ms ticks really count, rounded up
	// 0.01 gets rounded up to 0.032 ms or 1 tick
	// 0.3 / 0.032 = 9.375 -> 10 ticks
	// 9 + 1 -> 9 * 0.032 + 1 * 0.032 = 0.288 + 0.032 ms
	stateName[10]                    = "ReloadStart";
	stateTimeoutValue[10]            = 0.288; //0.3
	stateScript[10]                  = "onReloadStart";
	stateTransitionOnTimeout[10]     = "DeathCheck";
	
	stateName[11]                    = "Reloaded";
	stateTimeoutValue[11]            = 0.4;
	stateScript[11]                  = "onReloaded";
	stateTransitionOnTimeout[11]     = "Ready";
	stateSequence[11]                = "Fire";
	stateSound[11]                   = pistolClickSound;
	stateTransitionOnNoAmmo[11]      = "Empty";

	// Don't know of a way to stop state animations/sounds from playing so let's add more checks
	// If player is dead this should stop them from completing the reload and playing an animation while dead
	stateName[12]                    = "DeathCheck";
	stateScript[12]                  = "TTO_onDeathCheck";
	stateTimeoutValue[12]            = 0.032; // necessary to sync state with animation and sound
	stateTransitionOnTimeout[12]     = "Reloaded";

	// Ammo - gun is completely empty, full reload
	// NoAmmo - some ammo left in gun, partial reload
	// ImageAmmo is set through TT_onUseLight when light key is pressed
	// Transitions are opposite of expected since onAmmo transition to ReloadWait has to be consistent with Empty
	// Otherwise FireCheck may desync the state with animation and sound if gun is in Ready while empty
	stateName[13]                    = "ManualReload";
	stateTransitionOnAmmo[13]        = "ReloadWait";
	stateTransitionOnNoAmmo[13]      = "ReloadStart";

	stateName[14]                    = "Empty";
	stateTransitionOnLoaded[14]      = "Ready";
	stateTransitionOnAmmo[14]        = "ReloadWait";
	stateTransitionOnTriggerDown[14] = "FireCheckA";

	stateName[15]                    = "EmptyFire";
	stateScript[15]                  = "TTO_onEmptyFire";
	stateTransitionOnLoaded[15]      = "Ready";
	stateTransitionOnAmmo[15]        = "ReloadWait";
	stateTransitionOnTriggerUp[15]   = "Empty";
};

function VeteranPistolImage::onFire(%this,%obj,%slot)
{

	%obj.stopAudio(1);
	%obj.playAudio(1, VeteranPistolFireSound);

	if($Pref::Server::TTO::Recoil)
		%obj.spawnExplosion(TTOLittleRecoilProjectile,"1 1 1");
    
	if(vectorLen(%obj.getVelocity()) > 0.1)
	{
		%spread = 0.0008;
	}
	else
	{
		%spread = 0.0005;
	}
	%shellCount = 1;    
	%projectile = %this.projectile;
    
	%this.TTO_decrementAmmo(%obj);
	 %this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftRight);
	%obj.playThread(3, shiftLeft);
	return TTO_createProjectile(%this, %obj, %slot, %projectile, %shellCount, %spread);
}

function VeteranPistolImage::onReloadWait(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftUp);
	serverPlay3D(ReloadOut6Sound,%obj.getPosition());
}

function VeteranPistolImage::onReloadStart(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftLeft);
	serverPlay3D(ReloadTap3Sound,%obj.getPosition());
}

function VeteranPistolImage::onBounce(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
}

function VeteranPistolImage::onReloaded(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return %obj.setImageLoaded(%slot, 1);
	%this.TTO_reload(%obj, %slot, ReloadClick3Sound, plant);
	%this.TTO_displayAmmo(%obj);
}
