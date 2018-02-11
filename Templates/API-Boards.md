# `Board`

Represents a board.

## Constructors

### `Board(string id, TrelloAuthorization auth)`

Creates a new instance of the [Manatee.Trello.Board]() object.

**Parameter:** `id`

The board's ID.

**Parameter:** `auth`

(Optional) Custom authorization parameters. When not provided,
            [Manatee.Trello.TrelloAuthorization.Default]() will be used.

## Properties

### `ReadOnlyActionCollection Actions { get; }`

Gets the collection of actions performed on and within this board.

### `ReadOnlyCardCollection Cards { get; }`

Gets the collection of cards contained within this board.

#### Remarks

This property only exposes unarchived cards.

### `DateTime CreationDate { get; }`

Gets the creation date of the board.

### `string Description { get; set; }`

Gets or sets the board's description.

### `string Id { get; }`

Gets the board's ID.

### `bool? IsClosed { get; set; }`

Gets or sets whether this board is closed.

### `bool? IsSubscribed { get; set; }`

Gets or sets whether the current member is subscribed to this board.

### `BoardLabelCollection Labels { get; }`

Gets the collection of labels for this board.

### `ListCollection Lists { get; }`

Gets the collection of lists on this board.

#### Remarks

This property only exposes unarchived lists.

### `ReadOnlyMemberCollection Members { get; }`

Gets the collection of members on this board.

### `BoardMembershipCollection Memberships { get; }`

Gets the collection of members and their privileges on this board.

### `string Name { get; set; }`

Gets or sets the board's name.

### `Organization Organization { get; set; }`

Gets or sets the organization to which this board belongs.

#### Remarks

Setting null makes the board's first admin the owner.

### `ReadOnlyPowerUpCollection PowerUps { get; }`

Gets metadata about any active power-ups.

### `ReadOnlyPowerUpDataCollection PowerUpData { get; }`

Gets specific data regarding power-ups.

### `BoardPreferences Preferences { get; }`

Gets the set of preferences for the board.

### `BoardPersonalPreferences PersonalPreferences { get; }`

Gets the set of preferences for the board.

### `string Url { get; }`

Gets the board's URI.

## Events

### `event Action<Board, IEnumerable<string>> Updated`

Raised when data on the board is updated.

## Methods

### `void Delete()`

Deletes the card.

#### Remarks

This permanently deletes the card from Trello's server, however, this object will
            remain in memory and all properties will remain accessible.

### `void Refresh()`

Marks the board to be refreshed the next time data is accessed.

### `string ToString()`

Returns a string that represents the current object.

**Returns:** A string that represents the current object.

#### Filterpriority

2

# `BoardMembership`

Represents the permission level a member has on a board.

## Properties

### `DateTime CreationDate { get; }`

Gets the creation date of the membership.

### `string Id { get; }`

Gets the membership definition's ID.

### `bool? IsDeactivated { get; }`

Gets whether the member has accepted the invitation to join Trello.

### `Member Member { get; }`

Gets the member.

### `BoardMembershipType? MemberType { get; set; }`

Gets the membership's permission level.

## Events

### `event Action<BoardMembership, IEnumerable<string>> Updated`

Raised when data on the membership is updated.

## Methods

### `void Refresh()`

Marks the board membership to be refreshed the next time data is accessed.

