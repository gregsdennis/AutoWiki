# `Board`
Represents a board.

## Properties
### `Actions`

**Type:** `ReadOnlyActionCollection`

Gets the collection of actions performed on and within this board.


### `Cards`

**Type:** `ReadOnlyCardCollection`

Gets the collection of cards contained within this board.

### Remarks

This property only exposes unarchived cards.


### `CreationDate`

**Type:** `DateTime`

Gets the creation date of the board.


### `Description`

**Type:** `String`

Gets or sets the board's description.


### `Id`

**Type:** `String`

Gets the board's ID.


### `IsClosed`

**Type:** `Nullable<Boolean>`

Gets or sets whether this board is closed.


### `IsSubscribed`

**Type:** `Nullable<Boolean>`

Gets or sets whether the current member is subscribed to this board.


### `Labels`

**Type:** `BoardLabelCollection`

Gets the collection of labels for this board.


### `Lists`

**Type:** `ListCollection`

Gets the collection of lists on this board.

### Remarks

This property only exposes unarchived lists.


### `Members`

**Type:** `ReadOnlyMemberCollection`

Gets the collection of members on this board.


### `Memberships`

**Type:** `BoardMembershipCollection`

Gets the collection of members and their privileges on this board.


### `Name`

**Type:** `String`

Gets or sets the board's name.


### `Organization`

**Type:** `Organization`

Gets or sets the organization to which this board belongs.

### Remarks

Setting null makes the board's first admin the owner.


### `PowerUps`

**Type:** `ReadOnlyPowerUpCollection`

Gets metadata about any active power-ups.


### `PowerUpData`

**Type:** `ReadOnlyPowerUpDataCollection`

Gets specific data regarding power-ups.


### `Preferences`

**Type:** `BoardPreferences`

Gets the set of preferences for the board.


### `PersonalPreferences`

**Type:** `BoardPersonalPreferences`

Gets the set of preferences for the board.


### `Url`

**Type:** `String`

Gets the board's URI.


## Events
### `Updated`

Raised when data on the board is updated.


### `Updated`

Raised when data on the board is updated.


## Methods
### `Delete`

Deletes the card.

### Remarks

This permanently deletes the card from Trello's server, however, this object will
            remain in memory and all properties will remain accessible.


### `Refresh`

Marks the board to be refreshed the next time data is accessed.


### `ToString`

Returns a string that represents the current object.

### Returns

A string that represents the current object.

### Filterpriority

2


# `BoardMembership`
Represents the permission level a member has on a board.

## Properties
### `CreationDate`

**Type:** `DateTime`

Gets the creation date of the membership.


### `Id`

**Type:** `String`

Gets the membership definition's ID.


### `IsDeactivated`

**Type:** `Nullable<Boolean>`

Gets whether the member has accepted the invitation to join Trello.


### `Member`

**Type:** `Member`

Gets the member.


### `MemberType`

**Type:** `Nullable<BoardMembershipType>`

Gets the membership's permission level.


## Events
### `Updated`

Raised when data on the membership is updated.


### `Updated`

Raised when data on the membership is updated.


## Methods
### `Refresh`

Marks the board membership to be refreshed the next time data is accessed.


