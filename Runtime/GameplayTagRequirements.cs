﻿using System;
using UnityEngine;

namespace BandoWare.GameplayTags
{
   [Serializable]
   public struct GameplayTagRequirements
   {
      [SerializeField]
      internal GameplayTagContainer m_ForbiddenTags;

      [SerializeField]
      internal GameplayTagContainer m_RequiredTags;

      public bool IsEmpty
      {
         get => (m_ForbiddenTags == null || m_ForbiddenTags.IsEmpty) &&
               (m_RequiredTags == null || m_RequiredTags.IsEmpty);
      }

      public GameplayTagRequirements(GameplayTagContainer forbiddenTags, GameplayTagContainer requiredTags)
      {
         m_ForbiddenTags = forbiddenTags;
         m_RequiredTags = requiredTags;
      }

      public readonly bool Matches<T>(in T container) where T : IGameplayTagContainer
      {
         return !container.HasAny(m_ForbiddenTags) && container.HasAll(m_RequiredTags);
      }

      public readonly bool Matches<T, U>(in T staticContainer, in U dynamicContainer) where T : IGameplayTagContainer where U : IGameplayTagContainer
      {
         bool hasAnyForbiddenTag = staticContainer.HasAny(m_ForbiddenTags) || dynamicContainer.HasAny(m_ForbiddenTags);
         if (hasAnyForbiddenTag)
         {
            return false;
         }

         return GameplayTagContainerUtility.HasAll(staticContainer, dynamicContainer, m_RequiredTags);
      }
   }

   public static class GameplayTagRequirementsExtensionMethods
   {
      public static ref GameplayTagContainer GetForbiddenTags(ref this GameplayTagRequirements requirements)
      {
         return ref requirements.m_ForbiddenTags;
      }

      public static ref GameplayTagContainer GetRequiredTags(ref this GameplayTagRequirements requirements)
      {
         return ref requirements.m_RequiredTags;
      }
   }
}