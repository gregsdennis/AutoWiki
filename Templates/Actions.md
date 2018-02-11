# `Action`
Represents an action performed on Trello objects.

## Properties
### `CreationDate`

**Type:** `DateTime`

Gets the creation date of the action.


### `Creator`

**Type:** `Member`

Gets the member who performed the action.


### `Data`

**Type:** `ActionData`

Gets any data associated with the action.


### `Date`

**Type:** `Nullable<DateTime>`

Gets the date and time at which the action was performed.


### `Id`

**Type:** `String`

Gets the action's ID.


### `Type`

**Type:** `Nullable<ActionType>`

Gets the type of action.


## Events
### `Updated`

Raised when data on the action is updated.


### `Updated`

Raised when data on the action is updated.


## Methods
### `Delete`

Deletes the card.

### Remarks

This permanently deletes the card from Trello's server, however, this object will
            remain in memory and all properties will remain accessible.


### `ToString`

Returns a string that represents the current object.

### Returns

A string that represents the current object.

### Filterpriority

2


# `ActionData`
Exposes any data associated with an action.

## Properties
### `Attachment`

**Type:** `Attachment`

Gets an assocated attachment.


### `Board`

**Type:** `Board`

Gets an assocated board.


### `BoardSource`

**Type:** `Board`

Gets an assocated board.


### `BoardTarget`

**Type:** `Board`

Gets an assocated board.


### `Card`

**Type:** `Card`

Gets an assocated card.


### `CardSource`

**Type:** `Card`

Gets an assocated card.


### `CheckItem`

**Type:** `CheckItem`

Gets an assocated checklist item.


### `CheckList`

**Type:** `CheckList`

Gets an assocated checklist.


### `LastEdited`

**Type:** `Nullable<DateTime>`

Gets the date/time a comment was last edited.


### `List`

**Type:** `List`

Gets an assocated list.


### `ListAfter`

**Type:** `List`

Gets the current list.

### Remarks

For some action types, this information may be in the [Manatee.Trello.ActionData.List]()
            or [Manatee.Trello.ActionData.OldList]() properties.


### `ListBefore`

**Type:** `List`

Gets the previous list.

### Remarks

For some action types, this information may be in the [Manatee.Trello.ActionData.List]()
            or [Manatee.Trello.ActionData.OldList]() properties.


### `Member`

**Type:** `Member`

Gets an assocated member.


### `OldDescription`

**Type:** `String`

Gets the previous description.


### `OldList`

**Type:** `List`

Gets the previous list.

### Remarks

For some action types, this information may be in the [Manatee.Trello.ActionData.ListAfter]()
            or [Manatee.Trello.ActionData.ListBefore]() properties.


### `OldPosition`

**Type:** `Position`

Gets the previous position.


### `OldText`

**Type:** `String`

Gets the previous text value.


### `Organization`

**Type:** `Organization`

Gets an associated organization.


### `PowerUp`

**Type:** `PowerUpBase`

Gets an associated power-up.


### `Text`

**Type:** `String`

Gets assocated text.


### `WasArchived`

**Type:** `Nullable<Boolean>`

Gets whether the object was previously archived.


### `Value`

**Type:** `String`

Gets a custom value associate with the action if any.


# `ActionType`
Enumerates known types of [Manatee.Trello.Action]()s.

## Fields
### `Unknown`

Not recognized.  May have been created since the current version of this API.

### Remarks

This value is not supported by Trello's API.


### `AddAttachmentToCard`

Indicates an [Manatee.Trello.Attachment]() was added to a [Manatee.Trello.Card]().


### `AddChecklistToCard`

Indicates a [Manatee.Trello.CheckList]() was added to a [Manatee.Trello.Card]().


### `AddMemberToBoard`

Indicates a [Manatee.Trello.Member]() was added to a [Manatee.Trello.Board]().


### `AddMemberToCard`

Indicates a [Manatee.Trello.Member]() was added to a [Manatee.Trello.Card]().


### `AddMemberToOrganization`

Indicates a [Manatee.Trello.Member]() was added to an [Manatee.Trello.Organization]().


### `AddToOrganizationBoard`

Indicates a [Manatee.Trello.Organization]() was added to a [Manatee.Trello.Board]().


### `CommentCard`

Indicates a comment was added to a [Manatee.Trello.Card]().


### `ConvertToCardFromCheckItem`

Indicates a [Manatee.Trello.CheckList]() item was converted to [Manatee.Trello.Card]().


### `CopyBoard`

Indicates a [Manatee.Trello.Board]() was copied.


### `CopyCard`

Indicates a [Manatee.Trello.Card]() was copied.


### `CopyCommentCard`

Indicates a comment was copied from one [Manatee.Trello.Card]() to another.


### `CreateBoard`

Indicates a [Manatee.Trello.Board]() was created.


### `CreateCard`

Indicates a [Manatee.Trello.Card]() was created.


### `CreateList`

Indicates a [Manatee.Trello.List]() was created.


### `CreateOrganization`

Indicates an [Manatee.Trello.Organization]() was created.


### `DeleteAttachmentFromCard`

Indicates an [Manatee.Trello.Attachment]() was deleted from a [Manatee.Trello.Card]().


### `DeleteBoardInvitation`

Indicates an invitation to a [Manatee.Trello.Board]() was rescinded.


### `DeleteCard`

Indicates a [Manatee.Trello.Card]() was deleted.


### `DeleteOrganizationInvitation`

Indicates an invitation to an [Manatee.Trello.Organization]() was rescinded.


### `DisablePowerUp`

Indicates a power-up was disabled.


### `EmailCard`

Indicates a [Manatee.Trello.Card]() was created via email.


### `EnablePowerUp`

Indicates a power-up was enabled.


### `MakeAdminOfBoard`

Indicates a [Manatee.Trello.Member]() was made an admin of a [Manatee.Trello.Board]().


### `MakeNormalMemberOfBoard`

Indicates a [Manatee.Trello.Member]() was made a normal [Manatee.Trello.Member]() of a [Manatee.Trello.Board]().


### `MakeNormalMemberOfOrganization`

Indicates a [Manatee.Trello.Member]() was made a normal [Manatee.Trello.Member]() of an [Manatee.Trello.Organization]().


### `MakeObserverOfBoard`

Indicates a [Manatee.Trello.Member]() was made an observer of a [Manatee.Trello.Board]().


### `MemberJoinedTrello`

Indicates a [Manatee.Trello.Member]() joined Trello.


### `MoveCardFromBoard`

Indicates a [Manatee.Trello.Card]() was moved from one [Manatee.Trello.Board]() to another.


### `MoveCardToBoard`

Indicates a [Manatee.Trello.Card]() was moved from one [Manatee.Trello.Board]() to another.


### `MoveListFromBoard`

Indicates a [Manatee.Trello.List]() was moved from one [Manatee.Trello.Board]() to another.


### `MoveListToBoard`

Indicates a [Manatee.Trello.List]() was moved from one [Manatee.Trello.Board]() to another.


### `RemoveChecklistFromCard`

Indicates a [Manatee.Trello.CheckList]() was removed from a [Manatee.Trello.Card]().


### `RemoveFromOrganizationBoard`

Indicates an [Manatee.Trello.Organization]() was removed from a [Manatee.Trello.Board]().


### `RemoveMemberFromCard`

Indicates a [Manatee.Trello.Member]() was removed from a [Manatee.Trello.Card]().


### `UnconfirmedBoardInvitation`

Indicates an invitation to a [Manatee.Trello.Board]() was created.


### `UnconfirmedOrganizationInvitation`

Indicates an invitation to an [Manatee.Trello.Organization]() was created.


### `UpdateBoard`

Indicates a [Manatee.Trello.Board]() was updated.


### `UpdateCard`

Indicates a [Manatee.Trello.Card]() was updated.


### `UpdateCardClosed`

Indicates a [Manatee.Trello.Card]() was archived or unarchived.


### `UpdateCardDesc`

Indicates a [Manatee.Trello.Card]() description was updated.


### `UpdateCardIdList`

Indicates a [Manatee.Trello.Card]() was moved to a new [Manatee.Trello.List]().


### `UpdateCardName`

Indicates a [Manatee.Trello.Card]() name was updated.


### `UpdateCheckItemStateOnCard`

Indicates a [Manatee.Trello.CheckItem]() was checked or unchecked.


### `UpdateChecklist`

Indicates a [Manatee.Trello.CheckList]() was updated.


### `UpdateList`

Indicates a [Manatee.Trello.Member]() updated a [Manatee.Trello.List]().


### `UpdateListClosed`

Indicates a [Manatee.Trello.Member]() archived a [Manatee.Trello.List]().


### `UpdateListName`

Indicates a [Manatee.Trello.Member]() updated the name of a [Manatee.Trello.List]().


### `UpdateMember`

Indicates a [Manatee.Trello.Member]() was updated.


### `UpdateOrganization`

Indicates an [Manatee.Trello.Organization]() was updated.


### `EnablePlugin`

Indicates a plugin was enabled.


### `DisablePlugin`

Indicates a plugin was disabled.


### `DefaultForCardActions`

Indictes the default set of values returned by [Manatee.Trello.Card.Actions]().


### `All`

Indicates all action types


