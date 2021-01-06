namespace CRMLearning.Plugins.new_invoice
{
  /// <summary>
  /// Type depending on when it was called
  /// </summary>
  public enum RecalculateType
  {
    /// <summary>
    /// When new invoice was created with paid state
    /// </summary>
    Create,
    /// <summary>
    /// When invoice state was updated to paid
    /// </summary>
    Update,
    /// <summary>
    /// When invoice with paid state was deleted
    /// </summary>
    Delete
  }
}
